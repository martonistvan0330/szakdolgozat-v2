using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IPasswordRecoveryTokenRepository
{
    Task<Guid?> GetUserIdByActiveTokenAsync(string passwordRecoveryToken, CancellationToken cancellationToken = default);
    Task<PasswordRecoveryToken?> CreateAsync(Guid userId, string passwordRecoveryToken, CancellationToken cancellationToken = default);
    Task RevokeAsync(string passwordRecoveryToken, CancellationToken cancellationToken = default);
}