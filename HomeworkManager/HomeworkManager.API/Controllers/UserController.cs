using FluentValidation;
using HomeworkManager.API.Attributes;
using HomeworkManager.API.Extensions;
using HomeworkManager.API.Validation;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkManager.API.Controllers;

[HomeworkManagerAuthorize]
[ApiController]
[Route("api/User")]
public class UserController : ControllerBase
{
    private readonly EmailValidator _emailValidator;
    private readonly RoleValidator _roleValidator;
    private readonly UserIdValidator _userIdValidator;
    private readonly IUserManager _userManager;

    public UserController
    (
        EmailValidator emailValidator,
        RoleValidator roleValidator,
        UserIdValidator userIdValidator,
        IUserManager userManager
    )
    {
        _emailValidator = emailValidator;
        _roleValidator = roleValidator;
        _userIdValidator = userIdValidator;
        _userManager = userManager;
    }

    [HttpGet("Authenticate")]
    public async Task<ActionResult<UserModel>> Authenticate(CancellationToken cancellationToken = default)
    {
        return await _userManager.GetCurrentUserModelAsync(cancellationToken);
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<UserModel>> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var validationResult = await _userIdValidator.ValidateAsync
        (
            userId,
            options => { options.IncludeRuleSets("Default", UserIdValidator.IS_USER); },
            cancellationToken
        );

        if (!validationResult.IsValid)
        {
            return validationResult.ToActionResult();
        }

        var userModelResult = await _userManager.GetByIdAsync(userId, cancellationToken);

        return userModelResult.ToActionResult();
    }

    [HomeworkManagerAuthorize(Roles = Roles.ADMINISTRATOR)]
    [HttpGet]
    public async Task<ActionResult<Pageable<UserListRow>>> GetAllAsync([FromQuery] PageableOptions pageableOptions,
        CancellationToken cancellationToken = default)
    {
        return await _userManager.GetAllAsync(pageableOptions, cancellationToken);
    }

    [AllowAnonymous]
    [HttpGet("EmailAvailable")]
    public async Task<ActionResult<bool>> EmailAvailableAsync(string email, CancellationToken cancellationToken = default)
    {
        var validationResult = await _emailValidator.ValidateAsync(email, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToActionResult();
        }

        return await _userManager.EmailAvailableAsync(email, cancellationToken);
    }


    [AllowAnonymous]
    [HttpGet("UsernameAvailable")]
    public async Task<ActionResult<bool>> UsernameAvailableAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _userManager.UsernameAvailableAsync(username, cancellationToken);
    }


    [HomeworkManagerAuthorize(Roles = Roles.ADMINISTRATOR)]
    [HttpPut("{userId:guid}/Roles")]
    public async Task<ActionResult> UpdateRoles(Guid userId, ICollection<int> roleIds, CancellationToken cancellationToken = default)
    {
        var userIdValidationResult = await _userIdValidator.ValidateAsync(userId, cancellationToken);

        if (!userIdValidationResult.IsValid)
        {
            return userIdValidationResult.ToActionResult();
        }

        var rolesValidationResult = await _roleValidator.ValidateAsync(roleIds, cancellationToken);

        if (!rolesValidationResult.IsValid)
        {
            return rolesValidationResult.ToActionResult();
        }

        var roleUpdateResult = await _userManager.UpdateRoles(userId, roleIds, cancellationToken);

        return roleUpdateResult.ToActionResult();
    }
}