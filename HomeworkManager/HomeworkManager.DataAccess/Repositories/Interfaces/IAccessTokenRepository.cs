using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IAccessTokenRepository
{
    Task<AccessToken?> GetAsync(string accessToken, User user);
    Task<AccessToken?> RevokeAsync(string accessToken, User user);
}