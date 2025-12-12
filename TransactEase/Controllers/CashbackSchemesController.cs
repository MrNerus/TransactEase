using Dapper;
using Microsoft.AspNetCore.Mvc;
using TransactEase.BusinessLayer;
using TransactEase.Models.Entities;

namespace TransactEase.Controllers
{
    [ApiController]
    [Route("api/cashback-schemes")]
    public class CashbackSchemesController : ControllerBase
    {
        private readonly DbService _dbService;

        public CashbackSchemesController(DbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            using var connection = _dbService.CreateConnection();
            var sql = "SELECT * FROM cashback_schemes";
            var schemes = await connection.QueryAsync<CashbackScheme>(sql);
            return Ok(schemes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            using var connection = _dbService.CreateConnection();
            var sql = "SELECT * FROM cashback_schemes WHERE id = @Id";
            var scheme = await connection.QuerySingleOrDefaultAsync<CashbackScheme>(sql, new { Id = id });
            if (scheme == null)
                return NotFound();
            return Ok(scheme);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CashbackScheme scheme)
        {
            using var connection = _dbService.CreateConnection();
            var sql = @"
                INSERT INTO cashback_schemes (name, description, is_active)
                VALUES (@Name, @Description, @IsActive)
                RETURNING id;";
            var id = await connection.ExecuteScalarAsync<int>(sql, scheme);
            scheme.Id = id;
            return CreatedAtAction(nameof(GetById), new { id = scheme.Id }, scheme);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CashbackScheme scheme)
        {
            if (id != scheme.Id)
                return BadRequest();

            using var connection = _dbService.CreateConnection();
            var sql = @"
                UPDATE cashback_schemes 
                SET name = @Name, 
                    description = @Description, 
                    is_active = @IsActive 
                WHERE id = @Id";
            var result = await connection.ExecuteAsync(sql, scheme);
            if (result == 0)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using var connection = _dbService.CreateConnection();
            var sql = "DELETE FROM cashback_schemes WHERE id = @Id";
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            if (result == 0)
                return NotFound();
            return NoContent();
        }
    }
}
