using Dapper;
using TransactEase.DataLayer;
using TransactEase.Models.Entities;

namespace TransactEase.BusinessLayer;

public class TrustService
{
    private readonly DbService _dbService;
    private const int TrustedTransactionCount = 10;

    public TrustService(DbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<bool> IsTrustedUser(int userId)
    {
        using var connection = _dbService.CreateConnection();
        var sql = "SELECT COUNT(*) FROM transactions WHERE user_id = @UserId";
        var count = await connection.ExecuteScalarAsync<int>(sql, new { UserId = userId });
        return count >= TrustedTransactionCount;
    }

    public async Task<decimal> CalculateBalance(int userId)
    {
        using var connection = _dbService.CreateConnection();
        // Incoming funds: user is receiver
        // Outgoing funds: user is sender
        
        var incomingSql = "SELECT SUM(amount) FROM transactions WHERE receiver_id = @UserId";
        var outgoingSql = "SELECT SUM(amount) FROM transactions WHERE user_id = @UserId";

        var incoming = await connection.ExecuteScalarAsync<decimal?>(incomingSql, new { UserId = userId }) ?? 0;
        var outgoing = await connection.ExecuteScalarAsync<decimal?>(outgoingSql, new { UserId = userId }) ?? 0;

        return incoming - outgoing;
    }
}
