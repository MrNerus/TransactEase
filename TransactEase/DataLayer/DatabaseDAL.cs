using System.Data;
using Dapper;
using Npgsql;
using TransactEase.Models;

namespace TransactEase.DataLayer;

public class DatabaseDAL
{
    public async Task<int> GetDatabaseVersionAsync(IDbConnection connection)
    {
        try
        {
            return await connection.QuerySingleOrDefaultAsync<int>("SELECT Version FROM DB_VERSION");
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public async Task ApplyV1MigrationAsync(IDbConnection connection)
    {
        await using var transaction = (NpgsqlTransaction)await ((NpgsqlConnection)connection).BeginTransactionAsync();
        try
        {
            await connection.ExecuteAsync(Query.CreateOrganizationTable, transaction: transaction);
            await connection.ExecuteAsync(Query.CreateUsersTable, transaction: transaction);
            await connection.ExecuteAsync(Query.CreateRolesTable, transaction: transaction);
            await connection.ExecuteAsync(Query.CreateUserRolesTable, transaction: transaction);
            await connection.ExecuteAsync(Query.CreateDbVersionTable, transaction: transaction);
            await connection.ExecuteAsync("INSERT INTO DB_VERSION (Version) VALUES (1)", transaction: transaction);
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
