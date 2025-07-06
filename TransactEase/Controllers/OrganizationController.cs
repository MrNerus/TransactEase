using Microsoft.AspNetCore.Mvc;
using TransactEase.BusinessLayer;
using TransactEase.DTO;
using TransactEase.Enums;
using TransactEase.Models;

namespace TransactEase.Controllers;

[ApiController]
[Route("api/organization")]
public class OrganizationController(OrganizationHandler organizationHandler) : ControllerBase
{
    private OrganizationHandler _organizationHandler = organizationHandler;

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateOrganization([FromBody] OrganizationModel organization)
    {
        try
        {
            UserResponse res = await _organizationHandler.CreateOrganizationAsync(organization);
            if (res.Status != StatusEnum.ERROR) return Ok(res);
            return BadRequest(res);
        }
        catch (Exception e)
        { 
            return BadRequest(new UserResponse { Message = e.Message, Status = StatusEnum.ERROR });
        }
    }
}
