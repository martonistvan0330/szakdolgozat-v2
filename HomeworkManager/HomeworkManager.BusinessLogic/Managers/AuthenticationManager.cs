using System.Transactions;
using FluentResults;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.BusinessLogic.Services.Email.Interfaces;
using HomeworkManager.Model.Constants.Errors.Authentication;
using HomeworkManager.Model.CustomEntities.Authentication;
using HomeworkManager.Model.CustomEntities.Errors;
using HomeworkManager.Model.CustomEntities.User;

namespace HomeworkManager.BusinessLogic.Managers;

public class AuthenticationManager : IAuthenticationManager
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IEmailService _emailService;
    private readonly IJwtService _jwtService;
    private readonly ITokenService _tokenService;
    private readonly IUserManager _userManager;

    public AuthenticationManager(
        ICurrentUserService currentUserService,
        IEmailService emailService,
        IJwtService jwtService,
        ITokenService tokenService,
        IUserManager userManager
    )
    {
        _currentUserService = currentUserService;
        _emailService = emailService;
        _jwtService = jwtService;
        _tokenService = tokenService;
        _userManager = userManager;
    }

    public async Task<Result<AuthenticationResponse>> RegisterAsync(NewUser newUser, CancellationToken cancellationToken = default)
    {
        var createUserResult = await _userManager.CreateAsync(newUser, cancellationToken);

        if (!createUserResult.IsSuccess)
        {
            return createUserResult.ToResult();
        }

        var sendEmailResult = await SendEmailConfirmationAsync(createUserResult.Value, cancellationToken);

        if (!sendEmailResult.IsSuccess)
        {
            return sendEmailResult;
        }

        return await _jwtService.CreateTokensAsync(createUserResult.Value, cancellationToken);
    }

    public async Task<Result> ConfirmEmailAsync(EmailConfirmationRequest emailConfirmationRequest,
        CancellationToken cancellationToken = default)
    {
        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        var emailConfirmationResult = await _tokenService.CheckEmailConfirmationTokenAsync(userId, emailConfirmationRequest.Token, cancellationToken);

        if (!emailConfirmationResult.IsSuccess)
        {
            return emailConfirmationResult;
        }

        return await _userManager.ConfirmEmailAsync(userId, cancellationToken);
    }

    public async Task<Result<AuthenticationResponse>> LoginAsync(AuthenticationRequest authenticationRequest,
        CancellationToken cancellationToken = default)
    {
        var userResult = await _userManager.CheckPasswordAsync(authenticationRequest, cancellationToken);
        
        if (!userResult.IsSuccess)
        {
            return userResult.ToResult();
        }

        return await _jwtService.CreateTokensAsync(userResult.Value, cancellationToken);
    }

    public async Task<Result<AuthenticationResponse>> CreateRefreshTokenAsync
    (
        RefreshRequest refreshRequest,
        CancellationToken cancellationToken = default
    )
    {
        return await _jwtService.RefreshTokensAsync(refreshRequest, cancellationToken);
    }

    public async Task<Result> Logout(RevokeRequest tokens, CancellationToken cancellationToken = default)
    {

        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        return await _tokenService.RevokeTokensAsync(tokens.AccessToken, tokens.RefreshToken, userId, cancellationToken);
    }

    public async Task<Result> ResendEmailConfirmationAsync(CancellationToken cancellationToken = default)
    {
        var userModel = await _currentUserService.GetModelAsync(cancellationToken);
        
        if (userModel.EmailConfirmed)
        {
            return new BusinessError(AuthenticationErrorMessages.USER_EMAIL_ALREADY_CONFIRMED);
        }

        return await SendEmailConfirmationAsync(userModel, cancellationToken);
    }

    public async Task<Result> SendPasswordRecoveryEmailAsync(PasswordRecoveryRequest passwordRecoveryRequest,
        CancellationToken cancellationToken = default)
    {
        var userModel = await _currentUserService.GetModelAsync(cancellationToken);
        
        var createPasswordRecoveryTokenResult = await _tokenService.CreatePasswordRecoveryTokenAsync(userModel.UserId, cancellationToken);

        if (!createPasswordRecoveryTokenResult.IsSuccess)
        {
            return createPasswordRecoveryTokenResult.ToResult();
        }
        
        return await _emailService.SendPasswordRecoveryEmailAsync(userModel, createPasswordRecoveryTokenResult.Value, cancellationToken);
    }

    public async Task<Result> ResetPasswordAsync(PasswordResetRequest passwordResetRequest,
        CancellationToken cancellationToken = default)
    {
        using var transactionScope = new TransactionScope();
        
        var userIdResult = await _tokenService.GetUserIdByPasswordRecoveryTokenAsync(passwordResetRequest.Token, cancellationToken);

        if (!userIdResult.IsSuccess)
        {
            return userIdResult.ToResult();
        }
            
        var passwordUpdateResult = await _userManager.UpdatePasswordAsync(userIdResult.Value, passwordResetRequest.Password, cancellationToken);

        if (!passwordUpdateResult.IsSuccess)
        {
            return passwordUpdateResult;
        }
            
        transactionScope.Complete();
            
        return await _tokenService.RevokePasswordRecoveryTokenAsync(passwordResetRequest.Token, cancellationToken);
    }

    private async Task<Result> SendEmailConfirmationAsync(UserModel userModel, CancellationToken cancellationToken = default)
    {
        var createConfirmationTokenResult = await _tokenService.CreateEmailConfirmationTokenAsync(userModel.UserId, cancellationToken);

        if (!createConfirmationTokenResult.IsSuccess)
        {
            return createConfirmationTokenResult.ToResult();
        }

        return await _emailService.SendConfirmationEmailAsync(userModel, createConfirmationTokenResult.Value, cancellationToken);
    }
}