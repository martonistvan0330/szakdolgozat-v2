using HomeworkManager.API.Attributes;
using HomeworkManager.BusinessLogic.Managers;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.API.Controllers;

[HomeworkManagerAuthorize]
[ApiController]
[Route("api/User")]
public class UserController : ControllerBase
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager _userManager;

    public UserController(RoleManager<Role> roleManager, UserManager userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpGet("Authenticate")]
    public async Task<ActionResult<UserModel?>> Authenticate()
    {
        var user = await _userManager.GetByNameAsync(User.Identity!.Name!);

        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<UserModel?>> GetAsync(Guid userId)
    {
        var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

        if (currentUser is null)
        {
            return NotFound();
        }

        if (currentUser.Id != userId && !await _userManager.IsInRoleAsync(currentUser, Roles.ADMINISTRATOR))
        {
            return Forbid();
        }

        return await _userManager.GetByIdAsync(userId);
    }

    [HomeworkManagerAuthorize(Roles = Roles.ADMINISTRATOR)]
    [HttpGet]
    public async Task<ActionResult<Pageable<UserListRow>>> GetAllAsync([FromQuery] SortOptions sortOptions, [FromQuery] PageData pageData)
    {
        return await _userManager.GetAllAsync(sortOptions, pageData);
    }

    [HttpGet("Username")]
    public async Task<ActionResult<string>> GetUsernameAsync()
    {
        if (User.Identity?.Name is not null)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user?.UserName != null)
            {
                return user.UserName;
            }
        }

        return Unauthorized();
    }

    [AllowAnonymous]
    [HttpGet("EmailAvailable")]
    public async Task<ActionResult<bool>> EmailAvailableAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        return user is null;
    }

    [AllowAnonymous]
    [HttpGet("UsernameAvailable")]
    public async Task<ActionResult<bool>> UsernameAvailableAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        return user is null;
    }

    [HomeworkManagerAuthorize(Roles = Roles.ADMINISTRATOR)]
    [HttpPut("{userId}/Roles")]
    public async Task<ActionResult<bool>> UpdateRoles(string userId, ICollection<int> roleIds)
    {
        var roleDict = await _roleManager.Roles.ToDictionaryAsync(r => r.RoleId, r => r.Name!);
        var roles = roleIds.Select(id => roleDict[id]).ToHashSet();

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return NotFound();
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        var removeRoles = userRoles.Except(roles);
        var addRoles = roles.Except(userRoles);

        await _userManager.RemoveFromRolesAsync(user, removeRoles);
        await _userManager.AddToRolesAsync(user, addRoles);

        return true;
    }
}