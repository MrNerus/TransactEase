using Dapper;
using Microsoft.AspNetCore.Mvc;
using TransactEase.BusinessLayer;
using TransactEase.Models.Entities;

namespace TransactEase.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly DbService _dbService;

        public UsersController(DbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            using var connection = _dbService.CreateConnection();
            var sql = "SELECT * FROM users";
            var users = await connection.QueryAsync<User>(sql);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            using var connection = _dbService.CreateConnection();
            var sql = "SELECT * FROM users WHERE id = @Id";
            var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            using var connection = _dbService.CreateConnection();
            var sql = @"
                INSERT INTO users (full_name, email, organization_id, is_active)
                VALUES (@FullName, @Email, @OrganizationId, @IsActive)
                RETURNING id;";
            var id = await connection.ExecuteScalarAsync<int>(sql, user);
            user.Id = id;
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, User user)
        {
            if (id != user.Id)
                return BadRequest();

            using var connection = _dbService.CreateConnection();
            var sql = @"
                UPDATE users 
                SET full_name = @FullName, 
                    email = @Email, 
                    organization_id = @OrganizationId, 
                    is_active = @IsActive 
                WHERE id = @Id";
            var result = await connection.ExecuteAsync(sql, user);
            if (result == 0)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using var connection = _dbService.CreateConnection();
            var sql = "DELETE FROM users WHERE id = @Id";
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            if (result == 0)
                return NotFound();
            return NoContent();
        }
    }
}
