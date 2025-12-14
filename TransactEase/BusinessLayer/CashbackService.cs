using Dapper;
using TransactEase.DataLayer;
using TransactEase.Models.Entities;

namespace TransactEase.BusinessLayer;

public class CashbackService
{
    private readonly DbService _dbService;

    public CashbackService(DbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<decimal> CalculateCashback(decimal amount, int cashbackId)
    {
        using var connection = _dbService.CreateConnection();
        var sql = "SELECT * FROM cashback_schemes WHERE id = @Id";
        var scheme = await connection.QuerySingleOrDefaultAsync<CashbackScheme>(sql, new { Id = cashbackId });

        if (scheme == null || !scheme.IsActive)
        {
            return 0;
        }

        return amount * scheme.Rate;
    }
}
