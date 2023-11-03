using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.ErrorEntities;

namespace HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;

public interface ITokenService
{
    Task<Result<bool, BusinessError>> CheckTokensAsync(string accessToken, string refreshToken, Guid userId);
    Task AddTokensToUserAsync(string accessToken, string refreshToken, Guid userId);
    Task RevokeTokensAsync(string accessToken, string refreshToken, Guid userId);
    Task<string?> CreateEmailConfirmationTokenAsync(Guid userId);
    Task<string?> CreatePasswordRecoveryTokenAsync(Guid userId);
    Task<Result<bool, BusinessError>> CheckEmailConfirmationTokenAsync(Guid userId, string emailConfirmationToken);
    Task<Result<bool, BusinessError>> CheckPasswordRecoveryTokenAsync(Guid userId, string passwordRecoveryToken);
}