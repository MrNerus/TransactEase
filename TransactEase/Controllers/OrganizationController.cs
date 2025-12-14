using Microsoft.AspNetCore.Mvc;
using TransactEase.BusinessLayer;
using TransactEase.DTO;
using TransactEase.Enums;
using TransactEase.Models;
using TransactEase.Models.Entities;

namespace TransactEase.Controllers;

[ApiController]
[Route("api/organization")]
public class OrganizationController(OrganizationHandler organizationHandler) : ControllerBase
{
    private OrganizationHandler _organizationHandler = organizationHandler;

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateOrganization([FromBody] Organization organization)
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
    public async Task<IActionResult> GetAllOrganizations([FromQuery] SearchRequest? searchRequest = null)
    {
        try
        {
            UserResponse<PaginatedResponse<Organization>> res = await _organizationHandler.GetAllOrganizationsAsync(searchRequest);
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
    public async Task<IActionResult> GetOrganizationById(int id)
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
    [Route("update")]
    public async Task<IActionResult> UpdateOrganization([FromBody] Organization organization)
    {
        try
        {
            UserResponse res = await _organizationHandler.UpdateOrganizationAsync(organization);
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
    public async Task<IActionResult> DeleteOrganization(int id)
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
