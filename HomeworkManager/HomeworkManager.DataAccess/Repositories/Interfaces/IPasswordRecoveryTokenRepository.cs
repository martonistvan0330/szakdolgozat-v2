using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IPasswordRecoveryTokenRepository
{
    Task<string?> GetUserIdByActiveTokenAsync(string passwordRecoveryToken);
    Task<string?> CreateAsync(Guid userId, string passwordRecoveryToken);
    Task RevokeAsync(string passwordRecoveryToken);
}