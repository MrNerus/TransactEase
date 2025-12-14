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
        private readonly TrustService _trustService;
        private readonly CashbackService _cashbackService;

        public TransactionsController(DbService dbService, TrustService trustService, CashbackService cashbackService)
        {
            _dbService = dbService;
            _trustService = trustService;
            _cashbackService = cashbackService;
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

        [HttpGet("calculate-cashback")]
        public async Task<IActionResult> CalculateCashback([FromQuery] decimal amount, [FromQuery] int cashbackId)
        {
            var cashback = await _cashbackService.CalculateCashback(amount, cashbackId);
            return Ok(new { amount = cashback });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Transaction transaction)
        {
            // 1. Check Balance
            var currentBalance = await _trustService.CalculateBalance(transaction.UserId);
            var newBalance = currentBalance - transaction.Amount;

            if (newBalance < 0)
            {
                var isTrusted = await _trustService.IsTrustedUser(transaction.UserId);
                if (!isTrusted)
                {
                    return BadRequest("Insufficient funds. Overdraft not available for this user.");
                }
            }

            // 2. Calculate Cashback
            // Cashback not allowed if balance is negative (user is in overdraft)
            if (newBalance < 0)
            {
                transaction.CashbackAmount = 0;
            }
            else if (transaction.CashbackId.HasValue)
            {
                transaction.CashbackAmount = await _cashbackService.CalculateCashback(transaction.Amount, transaction.CashbackId.Value);
            }

            using var connection = _dbService.CreateConnection();
            var sql = @"
                INSERT INTO transactions (user_id, receiver_id, amount, cashback_id, organization_id, cashback_amount)
                VALUES (@UserId, @ReceiverId, @Amount, @CashbackId, @OrganizationId, @CashbackAmount)
                RETURNING id;";
            var id = await connection.ExecuteScalarAsync<int>(sql, transaction);
            transaction.Id = id;
            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
        }
    }
}
