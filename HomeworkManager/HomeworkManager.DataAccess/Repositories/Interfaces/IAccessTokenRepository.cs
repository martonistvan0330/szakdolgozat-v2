using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IAccessTokenRepository
{
    Task<AccessToken?> GetAsync(string accessToken, Guid userId, CancellationToken cancellationToken = default);
    Task<AccessToken> CreateAsync(string accessToken, Guid userId, CancellationToken cancellationToken = default);
    Task<AccessToken?> RevokeAsync(string accessToken, Guid userId, CancellationToken cancellationToken = default);
}