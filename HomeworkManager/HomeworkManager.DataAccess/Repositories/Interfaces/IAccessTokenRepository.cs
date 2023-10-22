using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IAccessTokenRepository
{
    Task<AccessToken?> GetAsync(string accessToken, Guid userId);
    Task CreateAsync(string accessToken, string refreshToken, Guid userId);
    Task<AccessToken?> RevokeAsync(string accessToken, Guid userId);
}