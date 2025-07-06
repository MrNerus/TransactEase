using Microsoft.AspNetCore.Mvc;
using TransactEase.BusinessLayer;
using TransactEase.DTO;
using TransactEase.Enums;
using TransactEase.Models;

namespace TransactEase.Controllers;

[ApiController]
[Route("api/create")]
public class CreateController(BankHandeler bankHandeler) : ControllerBase
{
    private BankHandeler _bankHandeler = bankHandeler;

    [HttpPost]
    [Route("bank")]
    public async Task<IActionResult> CreateBank([FromBody] BankModel bank)
    {
        try {
            UserResponse res = await _bankHandeler.CreateBankAsync(bank);
            if (res.Status != StatusEnum.ERROR) return Ok(res);
            return BadRequest(res);
        } catch (Exception e) {
            return BadRequest(new UserResponse { Message = e.Message, Status = StatusEnum.ERROR });
        }
    }
}
