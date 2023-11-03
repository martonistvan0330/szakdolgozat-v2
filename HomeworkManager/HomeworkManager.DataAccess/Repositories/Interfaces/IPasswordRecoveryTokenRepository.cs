using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IPasswordRecoveryTokenRepository
{
    Task<PasswordRecoveryToken?> GetActiveByUserAsync(Guid userId);
    Task<string?> CreateAsync(Guid userId, string passwordRecoveryToken);
    Task RevokeAsync(PasswordRecoveryToken passwordRecoveryToken);
}