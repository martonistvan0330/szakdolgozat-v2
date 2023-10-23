using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Authentication;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;
using HomeworkManager.Model.ErrorEntities;
using HomeworkManager.Model.ErrorEntities.Authentication;
using Microsoft.AspNetCore.Identity;

namespace HomeworkManager.BusinessLogic.Managers;

public class AuthenticationManager : IAuthenticationManager
{
    private readonly IJwtService _jwtService;
    private readonly ITokenService _tokenService;
    private readonly UserManager _userManager;

    public AuthenticationManager(
        IJwtService jwtService,
        ITokenService tokenService,
        UserManager userManager
    )
    {
        _jwtService = jwtService;
        _tokenService = tokenService;
        _userManager = userManager;
    }

    public async Task<Result<AuthenticationResponse, BusinessError>> RegisterAsync(NewUser newUser)
    {
        User user = new() { UserName = newUser.Username, Email = newUser.Email };

        var createResult = await _userManager.CreateAsync(
            user,
            newUser.Password
        );

        if (!createResult.Succeeded)
        {
            return new BusinessError(createResult.Errors.Select<IdentityError, string>(e => e.Description).ToArray());
        }

        var addToRoleResult = await _userManager.AddToRoleAsync(user, Roles.STUDENT);

        if (!addToRoleResult.Succeeded)
        {
            return new BusinessError(createResult.Errors.Select(e => e.Description).ToArray());
        }

        return await _jwtService.CreateTokensAsync(user);
    }

    public async Task<Result<AuthenticationResponse, BusinessError>> LoginAsync(AuthenticationRequest authenticationRequest)
    {
        var user = await _userManager.FindByNameAsync(authenticationRequest.Username);

        if (user is null)
        {
            return new BusinessError(AuthenticationErrorMessages.INVALID_USERNAME);
        }

        var validPassword = await _userManager.CheckPasswordAsync(user, authenticationRequest.Password);

        if (!validPassword)
        {
            return new BusinessError(AuthenticationErrorMessages.INVALID_PASSWORD);
        }

        return await _jwtService.CreateTokensAsync(user);
    }

    public async Task<Result<AuthenticationResponse, BusinessError>> CreateRefreshTokenAsync(
        string accessToken,
        string refreshToken
    )
    {
        return await _jwtService.RefreshTokensAsync(accessToken, refreshToken);
    }

    public async Task<Result<bool, BusinessError>> Logout(string? username, RevokeRequest tokens)
    {
        if (username is null)
        {
            return new BusinessError(AuthenticationErrorMessages.INVALID_USERNAME);
        }

        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
        {
            return new BusinessError(AuthenticationErrorMessages.INVALID_USERNAME);
        }

        await _tokenService.RevokeTokensAsync(tokens.AccessToken, tokens.RefreshToken, user.Id);

        return true;
    }
}