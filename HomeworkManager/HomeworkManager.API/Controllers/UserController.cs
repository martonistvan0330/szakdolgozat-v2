using HomeworkManager.API.Attributes;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkManager.API.Controllers;

[HomeworkManagerAuthorize]
[ApiController]
[Route("api/User")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public UserController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("Authenticate")]
    public async Task<ActionResult<UserModel>> Authenticate()
    {
        if (User.Identity?.Name is not null)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user is not null)
            {
                return Ok(new UserModel
                {
                    UserName = user.UserName!,
                    Password = "*****",
                    Email = user.Email!
                });
            }
        }

        return Unauthorized();
    }

    [HttpGet("UserName")]
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