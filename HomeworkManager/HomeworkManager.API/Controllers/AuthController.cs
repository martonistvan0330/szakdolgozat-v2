using FluentValidation;
using HomeworkManager.API.Attributes;
using HomeworkManager.API.Extensions;
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
    private readonly AbstractValidator<EmailConfirmationRequest> _emailConfirmationRequestValidator;
    private readonly AbstractValidator<NewUser> _newUserValidator;
    private readonly AbstractValidator<PasswordRecoveryRequest> _passwordRecoveryRequestValidator;
    private readonly AbstractValidator<PasswordResetRequest> _passwordResetRequestValidator;
    private readonly AbstractValidator<RefreshRequest> _refreshRequestValidator;
    private readonly AbstractValidator<RevokeRequest> _revokeRequestValidator;

    public AuthController
    (
        IAuthenticationManager authenticationManager,
        AbstractValidator<EmailConfirmationRequest> emailConfirmationRequestValidator,
        AbstractValidator<NewUser> newUserValidator,
        AbstractValidator<PasswordRecoveryRequest> passwordRecoveryRequestValidator,
        AbstractValidator<PasswordResetRequest> passwordResetRequestValidator,
        AbstractValidator<RefreshRequest> refreshRequestValidator,
        AbstractValidator<RevokeRequest> revokeRequestValidator
    )
    {
        _authenticationManager = authenticationManager;
        _emailConfirmationRequestValidator = emailConfirmationRequestValidator;
        _newUserValidator = newUserValidator;
        _passwordRecoveryRequestValidator = passwordRecoveryRequestValidator;
        _passwordResetRequestValidator = passwordResetRequestValidator;
        _refreshRequestValidator = refreshRequestValidator;
        _revokeRequestValidator = revokeRequestValidator;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<AuthenticationResponse>> RegisterAsync(NewUser newUser, CancellationToken cancellationToken)
    {
        var validationResult = await _newUserValidator.ValidateAsync(newUser, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToActionResult();
        }

        var registerUserResult = await _authenticationManager.RegisterAsync(newUser, cancellationToken);

        return registerUserResult.ToActionResult();
    }

    [HttpPost("ConfirmEmail")]
    public async Task<ActionResult<bool>> ConfirmEmailAsync(EmailConfirmationRequest emailConfirmationRequest, CancellationToken cancellationToken)
    {
        var validationResult = await _emailConfirmationRequestValidator.ValidateAsync(emailConfirmationRequest, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToActionResult();
        }

        var confirmEmailResult = await _authenticationManager.ConfirmEmailAsync(emailConfirmationRequest, cancellationToken);

        return confirmEmailResult.ToActionResult();
    }

    [HttpPost("Login")]
    public async Task<ActionResult<AuthenticationResponse>> CreateBearerTokenAsync(AuthenticationRequest authenticationRequest,
        CancellationToken cancellationToken)
    {
        var authenticationResponseResult = await _authenticationManager.LoginAsync(authenticationRequest, cancellationToken);

        return authenticationResponseResult.ToActionResult();
    }

    [HttpPost("RefreshToken")]
    public async Task<ActionResult<AuthenticationResponse>> RefreshTokenAsync(RefreshRequest refreshRequest,
        CancellationToken cancellationToken)
    {
        var authenticationResult = await _authenticationManager.CreateRefreshTokenAsync(refreshRequest, cancellationToken);

        return authenticationResult.ToActionResult();
    }

    [HomeworkManagerAuthorize]
    [HttpPost("Logout")]
    public async Task<ActionResult<bool>> LogoutAsync(RevokeRequest revokeRequest, CancellationToken cancellationToken)
    {
        var logoutResult = await _authenticationManager.Logout(revokeRequest, cancellationToken);

        return logoutResult.ToActionResult();
    }

    [HomeworkManagerAuthorize]
    [HttpPatch("ResendConfirmation")]
    public async Task<ActionResult<bool>> ResendConfirmationEmailAsync(CancellationToken cancellationToken)
    {
        var resendConfirmationResult =
            await _authenticationManager.ResendEmailConfirmationAsync(cancellationToken);

        return resendConfirmationResult.ToActionResult();
    }

    [HttpPost("PasswordRecovery")]
    public async Task<ActionResult> RecoverPasswordAsync(PasswordRecoveryRequest passwordRecoveryRequest, CancellationToken cancellationToken)
    {
        var passwordRecoveryResult = await _authenticationManager.SendPasswordRecoveryEmailAsync(passwordRecoveryRequest, cancellationToken);

        return passwordRecoveryResult.ToActionResult();
    }

    [HttpPost("PasswordReset")]
    public async Task<ActionResult> ResetPasswordAsync(PasswordResetRequest passwordResetRequest, CancellationToken cancellationToken)
    {
        var validationResult = await _passwordResetRequestValidator.ValidateAsync(passwordResetRequest, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToActionResult();
        }

        var passwordResetResult = await _authenticationManager.ResetPasswordAsync(passwordResetRequest, cancellationToken);

        return passwordResetResult.ToActionResult();
    }
}