using Dapper;
using Microsoft.AspNetCore.Mvc;
using TransactEase.BusinessLayer;
using TransactEase.Models.Entities;

namespace TransactEase.Controllers
{
    [ApiController]
    [Route("api/organizations")]
    public class OrganizationsController : ControllerBase
    {
        private readonly DbService _dbService;

        public OrganizationsController(DbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            using var connection = _dbService.CreateConnection();
            var sql = "SELECT * FROM organizations";
            var orgs = await connection.QueryAsync<Organization>(sql);
            return Ok(orgs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            using var connection = _dbService.CreateConnection();
            var sql = "SELECT * FROM organizations WHERE id = @Id";
            var org = await connection.QuerySingleOrDefaultAsync<Organization>(sql, new { Id = id });
            if (org == null)
                return NotFound();
            return Ok(org);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Organization org)
        {
            using var connection = _dbService.CreateConnection();
            var sql = @"
                INSERT INTO organizations (name, parent_id)
                VALUES (@Name, @ParentId)
                RETURNING id;";
            var id = await connection.ExecuteScalarAsync<int>(sql, org);
            org.Id = id;
            return CreatedAtAction(nameof(GetById), new { id = org.Id }, org);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Organization org)
        {
            if (id != org.Id)
                return BadRequest();

            using var connection = _dbService.CreateConnection();
            var sql = @"
                UPDATE organizations 
                SET name = @Name, 
                    parent_id = @ParentId 
                WHERE id = @Id";
            var result = await connection.ExecuteAsync(sql, org);
            if (result == 0)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using var connection = _dbService.CreateConnection();
            var sql = "DELETE FROM organizations WHERE id = @Id";
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            if (result == 0)
                return NotFound();
            return NoContent();
        }
    }
}
