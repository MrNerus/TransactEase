using System.Data;
using Dapper;
using TransactEase.Enums;
using TransactEase.Models;
using TransactEase.Utility;

namespace TransactEase.DataLayer;

public class OrganizationDAL(DbConnectionModel dbConnection)
{
    private readonly DbConnectionModel _dbConnection = dbConnection;

    public async Task<bool> CreateOrganizationAsync(OrganizationModel organization, IDbConnection connection, IDbTransaction transaction)
    {
        try
        {

            string insertQuery = Query.CreateOrganization;

            int reader = await connection.ExecuteAsync(insertQuery, organization, transaction);

            if (reader == 1)
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
}
