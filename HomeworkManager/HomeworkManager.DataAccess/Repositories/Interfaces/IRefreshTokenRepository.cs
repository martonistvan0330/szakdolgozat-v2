using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetAsync(string refreshToken, AccessToken accessToken, CancellationToken cancellationToken = default);
    Task<RefreshToken> CreateAsync(string refreshToken, int accessTokenId, CancellationToken cancellationToken = default);
    Task<RefreshToken?> RevokeAsync(string refreshToken, AccessToken accessToken, CancellationToken cancellationToken = default);
}