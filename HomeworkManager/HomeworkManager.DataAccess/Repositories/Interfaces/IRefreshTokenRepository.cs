using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetAsync(AccessToken accessToken, string refreshToken);
        Task RevokeAsync(AccessToken accessToken, string refreshToken);
    }
}