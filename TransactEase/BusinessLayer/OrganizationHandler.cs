using Dapper;
using Npgsql;
using TransactEase.DataLayer;
using TransactEase.DTO;
using TransactEase.Enums;
using TransactEase.Helper;
using TransactEase.Models;
using TransactEase.Utility;

namespace TransactEase.BusinessLayer;

public class OrganizationHandler(OrganizationDAL organizationDal, DbConnectionModel dbConnection)
{
    private OrganizationDAL _organizationDal = organizationDal;
    private readonly DbConnectionModel _dbConnection = dbConnection;

    public async Task<UserResponse> CreateOrganizationAsync(OrganizationModel organization)
    {
        try
        {
            _ = organization.ValidateOrganizationModel();

            using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            if (!string.IsNullOrEmpty(organization.ParentCode))
            {
                string checkOrganizationHierarchyQuery = Query.CheckOrganizationHierarchy;
                int? type = await connection.QueryFirstAsync<int>(checkOrganizationHierarchyQuery, new { ParentCode = organization.ParentCode });
                if (type is null) throw new UserException(new { message = "Parent organization not found", invoker = "CreateOrganization", data = organization }, true, StatusEnum.ERROR);
                if (type > (int)organization.Type) throw new UserException(new { message = "Organization Type Hierarchy is not correct", invoker = "CreateOrganization", data = organization }, true, StatusEnum.ERROR);
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
}
