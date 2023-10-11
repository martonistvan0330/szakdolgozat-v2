using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<bool> UserHasTokenAsync(User user, string refreshToken);
    }
}