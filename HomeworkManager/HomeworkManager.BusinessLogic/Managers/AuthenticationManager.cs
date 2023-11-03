using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.BusinessLogic.Services.Email.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.Constants.Errors.Authentication;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Authentication;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;
using HomeworkManager.Model.ErrorEntities;
using Microsoft.AspNetCore.Identity;

namespace HomeworkManager.BusinessLogic.Managers;

public class AuthenticationManager : IAuthenticationManager
{
    private readonly IEmailService _emailService;
    private readonly IJwtService _jwtService;
    private readonly ITokenService _tokenService;
    private readonly UserManager _userManager;

    public AuthenticationManager(
        IEmailService emailService,
        IJwtService jwtService,
        ITokenService tokenService,
        UserManager userManager
    )
    {
        _emailService = emailService;
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

        await SendEmailConfirmationAsync(user);

        return await _jwtService.CreateTokensAsync(user);
    }

    public async Task<Result<bool, BusinessError>> ConfirmEmailAsync(string? username, EmailConfirmationRequest emailConfirmationRequest)
    {
        if (username is null)
        {
            return new BusinessError(AuthenticationErrorMessages.INVALID_USERNAME);
        }

        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            return new BusinessError(AuthenticationErrorMessages.USER_NOT_FOUND);
        }

        var emailConfirmationResult = await _tokenService.CheckEmailConfirmationTokenAsync(user.Id, emailConfirmationRequest.Token);

        if (emailConfirmationResult.Success)
        {
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
        }

        return emailConfirmationResult;
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
            return new BusinessError(AuthenticationErrorMessages.USER_NOT_FOUND);
        }

        await _tokenService.RevokeTokensAsync(tokens.AccessToken, tokens.RefreshToken, user.Id);

        return true;
    }

    public async Task<Result<bool, BusinessError>> ResendEmailConfirmationAsync(string? username)
    {
        if (username is null)
        {
            return new BusinessError(AuthenticationErrorMessages.INVALID_USERNAME);
        }

        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
        {
            return new BusinessError(AuthenticationErrorMessages.USER_NOT_FOUND);
        }

        if (user.EmailConfirmed)
        {
            return new BusinessError(AuthenticationErrorMessages.USER_EMAIL_ALREADY_CONFIRMED);
        }

        await SendEmailConfirmationAsync(user);

        return true;
    }

    public async Task SendPasswordRecoveryEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is not null)
        {
            var passwordRecoveryToken = await _tokenService.CreatePasswordRecoveryTokenAsync(user.Id);

            if (passwordRecoveryToken is not null)
            {
                await _emailService.SendPasswordRecoveryEmailAsync(user, passwordRecoveryToken);
            }
        }
    }

    private async Task SendEmailConfirmationAsync(User user)
    {
        var confirmationToken = await _tokenService.CreateEmailConfirmationTokenAsync(user.Id);

        if (confirmationToken is not null)
        {
            await _emailService.SendConfirmationEmailAsync(user, confirmationToken);
        }
    }
    
    public async Task ResetPasswordAsync(string password, string passwordRecoveryToken)
    {
        var userId = await _tokenService.GetUserIdByPasswordRecoveryTokenAsync(passwordRecoveryToken);

        if (userId is not null)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is not null)
            {
                var passwordHash = _userManager.PasswordHasher.HashPassword(user, password);
                user.PasswordHash = passwordHash;
                await _userManager.UpdateAsync(user);

                await _tokenService.RevokePasswordRecoveryTokenAsync(passwordRecoveryToken);
            }
        }
    }
}