using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Authentication;
using HomeworkManager.Model.Entities;
using HomeworkManager.Model.ErrorEntities;

namespace HomeworkManager.BusinessLogic.Services.Interfaces
{
    public interface IJwtService
    {
        Task<AuthenticationResponse> CreateTokensAsync(User user);
        Task<Result<AuthenticationResponse, BusinessError>> RefreshTokensAsync(string accessToken, string refreshToken);
    }
}