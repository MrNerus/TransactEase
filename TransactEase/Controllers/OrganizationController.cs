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

    [HttpGet]
    [Route("get-all")]
    public async Task<IActionResult> GetAllOrganizations()
    {
        try
        {
            UserResponse res = await _organizationHandler.GetAllOrganizationsAsync();
            if (res.Status != StatusEnum.ERROR) return Ok(res);
            return BadRequest(res);
        }
        catch (Exception e)
        {
            return BadRequest(new UserResponse { Message = e.Message, Status = StatusEnum.ERROR });
        }
    }

    [HttpGet]
    [Route("get/{id}")]
    public async Task<IActionResult> GetOrganizationById(string id)
    {
        try
        {
            UserResponse res = await _organizationHandler.GetOrganizationByIdAsync(id);
            if (res.Status != StatusEnum.ERROR) return Ok(res);
            return BadRequest(res);
        }
        catch (Exception e)
        {
            return BadRequest(new UserResponse { Message = e.Message, Status = StatusEnum.ERROR });
        }
    }

    [HttpPut]
    [Route("update/{id}")]
    public async Task<IActionResult> UpdateOrganization(string id, [FromBody] OrganizationModel organization)
    {
        try
        {
            UserResponse res = await _organizationHandler.UpdateOrganizationAsync(id, organization);
            if (res.Status != StatusEnum.ERROR) return Ok(res);
            return BadRequest(res);
        }
        catch (Exception e)
        {
            return BadRequest(new UserResponse { Message = e.Message, Status = StatusEnum.ERROR });
        }
    }

    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> DeleteOrganization(string id)
    {
        try
        {
            UserResponse res = await _organizationHandler.DeleteOrganizationAsync(id);
            if (res.Status != StatusEnum.ERROR) return Ok(res);
            return BadRequest(res);
        }
        catch (Exception e)
        {
            return BadRequest(new UserResponse { Message = e.Message, Status = StatusEnum.ERROR });
        }
    }
}
