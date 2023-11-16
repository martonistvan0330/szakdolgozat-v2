using FluentResults;

namespace HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;

public interface ITokenService
{
    Task<Result> CheckTokensAsync(string accessToken, string refreshToken, Guid userId, CancellationToken cancellationToken = default);
    Task<Result> AddTokensToUserAsync(string accessToken, string refreshToken, Guid userId, CancellationToken cancellationToken = default);
    Task<Result> RevokeTokensAsync(string accessToken, string refreshToken, Guid userId, CancellationToken cancellationToken = default);
    Task<Result<string>> CreateEmailConfirmationTokenAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<string>> CreatePasswordRecoveryTokenAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result> CheckEmailConfirmationTokenAsync(Guid userId, string emailConfirmationToken, CancellationToken cancellationToken = default);
    Task<Result<Guid>> GetUserIdByPasswordRecoveryTokenAsync(string passwordRecoveryToken, CancellationToken cancellationToken = default);
    Task<Result> RevokePasswordRecoveryTokenAsync(string passwordRecoveryToken, CancellationToken cancellationToken = default);
}