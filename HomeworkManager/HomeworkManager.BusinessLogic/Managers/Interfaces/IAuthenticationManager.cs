using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Authentication;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.ErrorEntities;

namespace HomeworkManager.BusinessLogic.Managers.Interfaces;

public interface IAuthenticationManager
{
    Task<Result<AuthenticationResponse, BusinessError>> RegisterAsync(NewUser newUser);
    Task<Result<bool, BusinessError>> ConfirmEmailAsync(string? username, EmailConfirmationRequest emailConfirmationRequest);
    Task<Result<AuthenticationResponse, BusinessError>> LoginAsync(AuthenticationRequest authenticationRequest);
    Task<Result<AuthenticationResponse, BusinessError>> CreateRefreshTokenAsync(string accessToken, string refreshToken);
    Task<Result<bool, BusinessError>> Logout(string? username, RevokeRequest tokens);
    Task<Result<bool, BusinessError>> ResendEmailConfirmationAsync(string? username);
    Task SendPasswordRecoveryEmailAsync(string email);
    Task ResetPasswordAsync(string password, string passwordRecoveryToken);
}