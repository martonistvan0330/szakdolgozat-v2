using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IAccessTokenRepository
{
    Task<AccessToken> GetAsync(User user, string accessToken);
    Task<AccessToken?> RevokeAsync(User user, string accessToken);
}