using System.Data;
using Dapper;
using TransactEase.Enums;
using TransactEase.Models;
using TransactEase.Utility;
using TransactEase.Helper;
using Npgsql;

namespace TransactEase.DataLayer;

public class OrganizationDAL(DbConnectionModel dbConnection)
{
    private readonly DbConnectionModel _dbConnection = dbConnection;

    public async Task<bool> CreateOrganizationAsync(OrganizationModel organization, IDbConnection connection, IDbTransaction transaction)
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

    public async Task<IEnumerable<OrganizationModel>> GetAllOrganizationsAsync()
    {
        using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
        return await connection.QueryAsync<OrganizationModel>(Query.GetAllOrganizations);
    }

    public async Task<OrganizationModel?> GetOrganizationByIdAsync(string id)
    {
        using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
        OrganizationModel? organization = await connection.QueryFirstOrDefaultAsync<OrganizationModel>(Query.GetOrganization, new { Id = id });
        return organization;

    }

    public async Task<bool> UpdateOrganizationAsync(OrganizationModel organization)
    {
        using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
        var rowsAffected = await connection.ExecuteAsync(Query.UpdateOrganization, organization);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteOrganizationAsync(string id)
    {
        using var connection = new NpgsqlConnection(_dbConnection.GetConnectionString());
        var rowsAffected = await connection.ExecuteAsync(Query.DeleteOrganization, new { Id = id });
        return rowsAffected > 0;
    }
}
