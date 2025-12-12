using Microsoft.AspNetCore.Mvc;
using TransactEase.BusinessLayer;
using TransactEase.DTO;
using TransactEase.Enums;
using TransactEase.Models;

namespace TransactEase.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(AuthHandler authHandler) : ControllerBase
{
    private readonly AuthHandler _authHandler = authHandler;

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        try
        {
            var res = await _authHandler.LoginAsync(login);
            if (res.Status != StatusEnum.ERROR) return Ok(res);
            return BadRequest(res);
        }
        catch (Exception e)
        {
            return BadRequest(new UserResponse { Message = e.Message, Status = StatusEnum.ERROR });
        }
    }
}
