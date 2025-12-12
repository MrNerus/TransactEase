using Microsoft.AspNetCore.Mvc;
using TransactEase.BusinessLayer;
using TransactEase.DTO;
using TransactEase.Enums;

namespace TransactEase.Controllers;

[ApiController]
[Route("api/database")]
public class DatabaseController(DatabaseHandler databaseHandler) : ControllerBase
{
    private readonly DatabaseHandler _databaseHandler = databaseHandler;

    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> UpdateDatabase([FromBody] string connectionString)
    {
        try
        {
            var res = await _databaseHandler.UpdateDatabaseAsync(connectionString);
            if (res.Status != StatusEnum.ERROR) return Ok(res);
            return BadRequest(res);
        }
        catch (Exception e)
        {
            return BadRequest(new UserResponse { Message = e.Message, Status = StatusEnum.ERROR });
        }
    }
}
