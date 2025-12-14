using System.Data;
using Dapper;
using TransactEase.Enums;
using TransactEase.Models;
using TransactEase.Models.Entities;
using TransactEase.DTO;
using TransactEase.Utility;
using TransactEase.Helper;
using Npgsql;
using TransactEase.BusinessLayer;

namespace TransactEase.DataLayer;

public class OrganizationDAL(DbService dbService)
{
    private readonly DbService _dbService = dbService;
    public async Task<bool> CreateOrganizationAsync(Organization organization, IDbConnection connection, IDbTransaction transaction)
    {
        try
        {

            string insertQuery = Query.CreateOrganization;

            int rowsAffected = await connection.ExecuteAsync(insertQuery, organization, transaction);

            if (rowsAffected == 1)
            {
                _ = new UserException(new { message = "Organization created successfully", invoker = "CreateOrganization", data = organization }, true, StatusEnum.SUCCESS);
                return true;
            }
            throw new UserException(new { message = "Error creating organization", invoker = "CreateOrganization", data = organization }, true, StatusEnum.ERROR);

        }
        catch (UserException e)
        {
            _ = e;
            throw;
        }
        catch (Exception e)
        {
            _ = new UserException(new { message = "Error creating organization", invoker = "CreateOrganization", data = new { DataIn = organization, Exception = e } }, true, StatusEnum.ERROR);
            throw;
        }
    }

    public async Task<PaginatedResponse<Organization>> GetAllOrganizationsAsync(SearchRequest? searchRequest)
    {
        using var conn = _dbService.CreateConnection();

        PaginatedResponse<Organization> paginatedResponse = await ListingService.GetPagedListAsync<Organization>(Query.GetAllOrganizations, searchRequest, FieldColumnMap.UserField, conn);
        return paginatedResponse;
    }

    public async Task<Organization?> GetOrganizationByIdAsync(int id)
    {
        using var connection = _dbService.CreateConnection();
        Organization? organization = await connection.QueryFirstOrDefaultAsync<Organization>(Query.GetOrganization, new { id = id });
        return organization;

    }

    public async Task<bool> UpdateOrganizationAsync(Organization organization)
    {
        using var connection = _dbService.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(Query.UpdateOrganization, organization);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteOrganizationAsync(int id)
    {
        using var connection = _dbService.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(Query.DeleteOrganization, new { id = id });
        return rowsAffected > 0;
    }
}
