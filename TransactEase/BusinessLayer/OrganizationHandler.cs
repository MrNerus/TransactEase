using Dapper;
using Npgsql;
using TransactEase.DataLayer;
using TransactEase.DTO;
using TransactEase.Enums;
using TransactEase.Helper;
using TransactEase.Models;
using TransactEase.Models.Entities;
using TransactEase.Utility;

namespace TransactEase.BusinessLayer;

public class OrganizationHandler(OrganizationDAL organizationDal, DbService dbService)
{
    private readonly DbService _dbService = dbService;
    private OrganizationDAL _organizationDal = organizationDal;
    public async Task<UserResponse> CreateOrganizationAsync(Organization organization)
    {
        try
        {

            using NpgsqlConnection connection = (NpgsqlConnection)_dbService.CreateConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            if (organization.ParentId is not null)
            {
                string checkOrganizationHierarchyQuery = Query.CheckOrganizationHierarchy;
                int? parentId = await connection.QueryFirstAsync<int>(checkOrganizationHierarchyQuery, new { ParentId = organization.ParentId }, transaction);
                if (parentId is null) throw new UserException(new { message = "Parent organization not found", invoker = "CreateOrganization", data = organization }, true, StatusEnum.ERROR);
            }

            try
            {
                _ = await _organizationDal.CreateOrganizationAsync(organization, connection, transaction);
            }
            catch (Exception e)
            {
                _ = e;
                await transaction.RollbackAsync();
                throw;
            }

            await transaction.CommitAsync();
            return new UserResponse { Message = "Organization created successfully", Status = StatusEnum.SUCCESS, Data = organization };

        }
        catch (Exception e)
        {
            return new UserResponse { Message = e.Message, Status = StatusEnum.ERROR, Data = organization };
        }
    }

    public async Task<UserResponse<PaginatedResponse<Organization>>> GetAllOrganizationsAsync(SearchRequest? searchRequest)
    {
        try
        {
            PaginatedResponse<Organization> organizations = await _organizationDal.GetAllOrganizationsAsync(searchRequest);
            if (organizations.Data == null || organizations.Data.Count == 0)
            {
                return new UserResponse<PaginatedResponse<Organization>> { Message = "No organizations found", Status = StatusEnum.INFO };
            }

            return new UserResponse<PaginatedResponse<Organization>> { Message = "Organizations retrieved successfully", Status = StatusEnum.SUCCESS, Data = organizations };
        }
        catch (Exception e)
        {
            return new UserResponse<PaginatedResponse<Organization>> { Message = e.Message, Status = StatusEnum.ERROR };
        }
    }

    public async Task<UserResponse> GetOrganizationByIdAsync(int id)
    {
        try
        {
            var organization = await _organizationDal.GetOrganizationByIdAsync(id);
            if (organization == null)
            {
                return new UserResponse { Message = "Organization not found", Status = StatusEnum.ERROR };
            }
            return new UserResponse { Message = "Organization retrieved successfully", Status = StatusEnum.SUCCESS, Data = organization };
        }
        catch (Exception e)
        {
            return new UserResponse { Message = e.Message, Status = StatusEnum.ERROR };
        }
    }

    public async Task<UserResponse> UpdateOrganizationAsync(Organization organization)
    {
        try
        {
            var result = await _organizationDal.UpdateOrganizationAsync(organization);
            if (!result)
            {
                return new UserResponse { Message = "Error updating organization", Status = StatusEnum.ERROR };
            }
            return new UserResponse { Message = "Organization updated successfully", Status = StatusEnum.SUCCESS, Data = organization };
        }
        catch (Exception e)
        {
            return new UserResponse { Message = e.Message, Status = StatusEnum.ERROR, Data = organization };
        }
    }

    public async Task<UserResponse> DeleteOrganizationAsync(int id)
    {
        try
        {
            var result = await _organizationDal.DeleteOrganizationAsync(id);
            if (!result)
            {
                return new UserResponse { Message = "Error deleting organization", Status = StatusEnum.ERROR };
            }
            return new UserResponse { Message = "Organization deleted successfully", Status = StatusEnum.SUCCESS };
        }
        catch (Exception e)
        {
            return new UserResponse { Message = e.Message, Status = StatusEnum.ERROR };
        }
    }
}
