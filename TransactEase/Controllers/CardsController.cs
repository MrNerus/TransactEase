using Dapper;
using Microsoft.AspNetCore.Mvc;
using TransactEase.BusinessLayer;
using TransactEase.Models.Entities;

namespace TransactEase.Controllers
{
    [ApiController]
    [Route("api/cards")]
    public class CardsController : ControllerBase
    {
        private readonly DbService _dbService;

        public CardsController(DbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            using var connection = _dbService.CreateConnection();
            var sql = "SELECT * FROM cards";
            var cards = await connection.QueryAsync<Card>(sql);
            return Ok(cards);
        }

        [HttpGet("{cardNumber}")]
        public async Task<IActionResult> GetById(string cardNumber)
        {
            using var connection = _dbService.CreateConnection();
            var sql = "SELECT * FROM cards WHERE card_number = @CardNumber";
            var card = await connection.QuerySingleOrDefaultAsync<Card>(sql, new { CardNumber = cardNumber });
            if (card == null)
                return NotFound();
            return Ok(card);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Card card)
        {
            using var connection = _dbService.CreateConnection();
            var sql = @"
                INSERT INTO cards (card_number, card_type, status, organization_id, user_id, issue_date, expiry_date, cvv)
                VALUES (@CardNumber, @CardType, @Status, @OrganizationId, @UserId, @IssueDate, @ExpiryDate, @Cvv)
                RETURNING card_number;";
            await connection.ExecuteAsync(sql, card);
            return CreatedAtAction(nameof(GetById), new { cardNumber = card.CardNumber }, card);
        }

        [HttpPut("{cardNumber}")]
        public async Task<IActionResult> Update(string cardNumber, Card card)
        {
            if (cardNumber != card.CardNumber)
                return BadRequest();

            using var connection = _dbService.CreateConnection();
            var sql = @"
                UPDATE cards 
                SET card_type = @CardType, 
                    status = @Status, 
                    organization_id = @OrganizationId, 
                    user_id = @UserId, 
                    issue_date = @IssueDate,
                    expiry_date = @ExpiryDate,
                    cvv = @Cvv
                WHERE card_number = @CardNumber";
            var result = await connection.ExecuteAsync(sql, card);
            if (result == 0)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{cardNumber}")]
        public async Task<IActionResult> Delete(string cardNumber)
        {
            using var connection = _dbService.CreateConnection();
            var sql = "DELETE FROM cards WHERE card_number = @CardNumber";
            var result = await connection.ExecuteAsync(sql, new { CardNumber = cardNumber });
            if (result == 0)
                return NotFound();
            return NoContent();
        }
    }
}
