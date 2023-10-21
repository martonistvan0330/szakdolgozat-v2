using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetAsync(string refreshToken, AccessToken accessToken);
    Task RevokeAsync(string refreshToken, AccessToken accessToken);
}