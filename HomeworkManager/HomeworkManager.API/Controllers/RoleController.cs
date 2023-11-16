using HomeworkManager.API.Attributes;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
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
    private readonly IRoleManager _roleManager;

    public RoleController(IRoleManager roleManager)
    {
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleModel>>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        return Ok(await _roleManager.GetAllAsync(cancellationToken));
    }
}