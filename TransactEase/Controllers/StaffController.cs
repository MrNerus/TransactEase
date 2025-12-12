using Dapper;
using Microsoft.AspNetCore.Mvc;
using TransactEase.BusinessLayer;
using TransactEase.Models.Entities;

namespace TransactEase.Controllers
{
    [ApiController]
    [Route("api/staff")]
    public class StaffController : ControllerBase
    {
        private readonly DbService _dbService;

        public StaffController(DbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            using var connection = _dbService.CreateConnection();
            var sql = "SELECT * FROM staff";
            var staffList = await connection.QueryAsync<Staff>(sql);
            return Ok(staffList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            using var connection = _dbService.CreateConnection();
            var sql = "SELECT * FROM staff WHERE id = @Id";
            var staff = await connection.QuerySingleOrDefaultAsync<Staff>(sql, new { Id = id });
            if (staff == null)
                return NotFound();
            return Ok(staff);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Staff staff)
        {
            using var connection = _dbService.CreateConnection();
            var sql = @"
                INSERT INTO staff (full_name, email, organization_id, role, is_active)
                VALUES (@FullName, @Email, @OrganizationId, @Role, @IsActive)
                RETURNING id;";
            var id = await connection.ExecuteScalarAsync<int>(sql, staff);
            staff.Id = id;
            return CreatedAtAction(nameof(GetById), new { id = staff.Id }, staff);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Staff staff)
        {
            if (id != staff.Id)
                return BadRequest();

            using var connection = _dbService.CreateConnection();
            var sql = @"
                UPDATE staff 
                SET full_name = @FullName, 
                    email = @Email, 
                    organization_id = @OrganizationId, 
                    role = @Role,
                    is_active = @IsActive
                WHERE id = @Id";
            var result = await connection.ExecuteAsync(sql, staff);
            if (result == 0)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using var connection = _dbService.CreateConnection();
            var sql = "DELETE FROM staff WHERE id = @Id";
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            if (result == 0)
                return NotFound();
            return NoContent();
        }
    }
}
