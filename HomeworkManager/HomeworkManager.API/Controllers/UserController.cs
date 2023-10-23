using HomeworkManager.API.Attributes;
using HomeworkManager.BusinessLogic.Managers;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.User;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkManager.API.Controllers;

[HomeworkManagerAuthorize]
[ApiController]
[Route("api/User")]
public class UserController : ControllerBase
{
    private readonly UserManager _userManager;

    public UserController(UserManager userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("Authenticate")]
    public async Task<ActionResult<UserModel>> Authenticate()
    {
        if (User.Identity?.Name is not null)
        {
            var user = await _userManager.GetByNameAsync(User.Identity.Name);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        return Unauthorized();
    }

    [HomeworkManagerAuthorize(Roles = Roles.ADMINISTRATOR)]
    [HttpGet]
    public async Task<ActionResult<Pageable<UserListRow>>> GetAll([FromQuery] SortOptions sortOptions, [FromQuery] PageData pageData)
    {
        return Ok(await _userManager.GetAllAsync(sortOptions, pageData));
    }

    [HttpGet("Username")]
    public async Task<ActionResult<string>> GetUsernameAsync()
    {
        if (User.Identity?.Name is not null)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user?.UserName != null)
            {
                return Ok(user.UserName);
            }
        }

        return Unauthorized();
    }
}