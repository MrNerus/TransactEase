using Dapper;
using Npgsql;
using TransactEase.DataLayer;
using TransactEase.DTO;
using TransactEase.Enums;

namespace TransactEase.BusinessLayer;

public class DatabaseHandler(DatabaseDAL databaseDal)
{
    private readonly DatabaseDAL _databaseDal = databaseDal;

    public async Task<UserResponse> UpdateDatabaseAsync(string connectionString)
    {
        try
        {
            await using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            var currentVersion = await _databaseDal.GetDatabaseVersionAsync(connection);

            if (currentVersion < 1)
            {
                await _databaseDal.ApplyV1MigrationAsync(connection);
            }

            return new UserResponse { Message = "Database updated successfully", Status = StatusEnum.SUCCESS };
        }
        catch (Exception e)
        {
            return new UserResponse { Message = e.Message, Status = StatusEnum.ERROR };
        }
    }
}
