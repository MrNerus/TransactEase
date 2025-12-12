using Dapper;
using Microsoft.AspNetCore.Mvc;
using TransactEase.BusinessLayer;
using TransactEase.Models.Entities;

namespace TransactEase.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly DbService _dbService;

        public ReportsController(DbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("generate")]
        public async Task<IActionResult> GenerateReport([FromQuery] string reportType, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            using var connection = _dbService.CreateConnection();
            var start = startDate ?? DateTime.MinValue;
            var end = endDate ?? DateTime.MaxValue;

            if (reportType == "report_1") // Transaction Report
            {
                var sql = "SELECT * FROM transactions WHERE created_at BETWEEN @Start AND @End";
                var data = await connection.QueryAsync<Transaction>(sql, new { Start = start, End = end });
                return Ok(data);
            }
            if (reportType == "report_2") // Cashback Report
            {
                 var sql = "SELECT * FROM transactions WHERE cashback_id IS NOT NULL AND created_at BETWEEN @Start AND @End";
                 var data = await connection.QueryAsync<Transaction>(sql, new { Start = start, End = end });
                 return Ok(data);
            }
            if (reportType == "report_3") // User Activity Report (using Audit Logs for simplicity)
            {
                var sql = "SELECT * FROM audit_logs WHERE timestamp BETWEEN @Start AND @End";
                var data = await connection.QueryAsync<AuditLog>(sql, new { Start = start, End = end });
                return Ok(data);
            }

            return BadRequest(new { message = "Invalid report type" });
        }
    }
}
