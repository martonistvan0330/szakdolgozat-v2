using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Authentication;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.ErrorEntities;

namespace HomeworkManager.BusinessLogic.Managers.Interfaces
{
    public interface IAuthenticationManager
    {
        Task<Result<UserModel, BusinessError>> RegisterAsync(UserModel newUser);
        Task<Result<AuthenticationResponse, BusinessError>> CreateBearerTokenAsync(string userName, string password);
        Task<Result<AuthenticationResponse, BusinessError>> CreateRefreshTokenAsync(string accessToken, string refreshToken);
    }
}