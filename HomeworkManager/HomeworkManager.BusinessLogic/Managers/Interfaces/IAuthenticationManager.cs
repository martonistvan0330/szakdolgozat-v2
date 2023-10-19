using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Authentication;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.ErrorEntities;

namespace HomeworkManager.BusinessLogic.Managers.Interfaces
{
    public interface IAuthenticationManager
    {
        Task<Result<AuthenticationResponse, BusinessError>> RegisterAsync(UserModel newUser);
        Task<Result<AuthenticationResponse, BusinessError>> LoginAsync(AuthenticationRequest authenticationRequest);
        Task<Result<AuthenticationResponse, BusinessError>> CreateRefreshTokenAsync(string accessToken, string refreshToken);
        Task<Result<bool, BusinessError>> Logout(string? userName, RevokeRequest tokens);
    }
}