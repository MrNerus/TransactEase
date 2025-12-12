using Dapper;
using Microsoft.AspNetCore.Mvc;
using TransactEase.BusinessLayer;
using TransactEase.Models.Entities;

namespace TransactEase.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly DbService _dbService;

        public TransactionsController(DbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            using var connection = _dbService.CreateConnection();
            var sql = "SELECT * FROM transactions";
            var transactions = await connection.QueryAsync<Transaction>(sql);
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            using var connection = _dbService.CreateConnection();
            var sql = "SELECT * FROM transactions WHERE id = @Id";
            var transaction = await connection.QuerySingleOrDefaultAsync<Transaction>(sql, new { Id = id });
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Transaction transaction)
        {
            using var connection = _dbService.CreateConnection();
            var sql = @"
                INSERT INTO transactions (user_id, receiver_id, amount, cashback_id, organization_id)
                VALUES (@UserId, @ReceiverId, @Amount, @CashbackId, @OrganizationId)
                RETURNING id;";
            var id = await connection.ExecuteScalarAsync<int>(sql, transaction);
            transaction.Id = id;
            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
        }
    }
}
