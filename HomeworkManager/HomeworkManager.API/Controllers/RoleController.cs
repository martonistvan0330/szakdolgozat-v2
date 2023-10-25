using HomeworkManager.API.Attributes;
using HomeworkManager.Model.CustomEntities.Role;
using HomeworkManager.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.API.Controllers;

[HomeworkManagerAuthorize]
[ApiController]
[Route("api/Role")]
public class RoleController : ControllerBase
{
    private readonly RoleManager<Role> _roleManager;

    public RoleController(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleModel>>> GetRolesAsync()
    {
        return Ok(await _roleManager.Roles.ToListAsync());
    }
}