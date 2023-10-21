using HomeworkManager.API.Attributes;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.CustomEntities.Authentication;
using HomeworkManager.Model.CustomEntities.User;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkManager.API.Controllers;

[ApiController]
[Route("api/Auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationManager _authenticationManager;

    public AuthController(
        IAuthenticationManager authenticationManager
    )
    {
        _authenticationManager = authenticationManager;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<AuthenticationResponse>> RegisterAsync(UserModel newUser)
    {
        var registerUserResult = await _authenticationManager.RegisterAsync(newUser);

        return registerUserResult.Match<ActionResult<AuthenticationResponse>>(
            result => Ok(result),
            error => BadRequest(error.Message)
        );
    }

    [HttpPost("Login")]
    public async Task<ActionResult<AuthenticationResponse>> CreateBearerTokenAsync(AuthenticationRequest authenticationRequest)
    {
        var authenticationResponseResult =
            await _authenticationManager.LoginAsync(authenticationRequest);

        return authenticationResponseResult.Match<ActionResult<AuthenticationResponse>>(
            result => Ok(result),
            error => Unauthorized(error.Message)
        );
    }

    [HttpPost("RefreshToken")]
    public async Task<ActionResult<AuthenticationResponse>> RefreshTokenAsync(RefreshRequest tokens)
    {
        var authenticationResult = await _authenticationManager.CreateRefreshTokenAsync(tokens.AccessToken, tokens.RefreshToken);

        return authenticationResult.Match<ActionResult<AuthenticationResponse>>(
            result => Ok(result),
            error => Unauthorized(error.Message)
        );
    }

    [HomeworkManagerAuthorize]
    [HttpPost("Logout")]
    public async Task<ActionResult<bool>> LogoutAsync(RevokeRequest tokens)
    {
        var logoutResult = await _authenticationManager.Logout(User.Identity?.Name, tokens);

        return logoutResult.Match<ActionResult<bool>>(
            result => Ok(result),
            error => Unauthorized(error.Message)
        );
    }
}