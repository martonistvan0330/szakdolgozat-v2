using FluentResults;
using HomeworkManager.Model.CustomEntities.Authentication;
using HomeworkManager.Model.CustomEntities.User;

namespace HomeworkManager.BusinessLogic.Managers.Interfaces;

public interface IAuthenticationManager
{
    Task<Result<AuthenticationResponse>> RegisterAsync(NewUser newUser, CancellationToken cancellationToken = default);

    Task<Result> ConfirmEmailAsync(EmailConfirmationRequest emailConfirmationRequest,
        CancellationToken cancellationToken = default);

    Task<Result<AuthenticationResponse>> LoginAsync(AuthenticationRequest authenticationRequest,
        CancellationToken cancellationToken = default);

    Task<Result<AuthenticationResponse>> CreateRefreshTokenAsync(RefreshRequest refreshRequest,
        CancellationToken cancellationToken = default);

    Task<Result> Logout(RevokeRequest tokens, CancellationToken cancellationToken = default);
    Task<Result> ResendEmailConfirmationAsync(CancellationToken cancellationToken = default);

    Task<Result> SendPasswordRecoveryEmailAsync(PasswordRecoveryRequest passwordRecoveryRequest,
        CancellationToken cancellationToken = default);

    Task<Result> ResetPasswordAsync(PasswordResetRequest passwordResetRequest,
        CancellationToken cancellationToken = default);
}