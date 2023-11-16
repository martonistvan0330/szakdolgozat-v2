using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IEmailConfirmationTokenRepository
{
    Task<bool> IsActiveAsync(string emailConfirmationToken, CancellationToken cancellationToken = default);
    Task<EmailConfirmationToken?> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<string> CreateAsync(Guid userId, string emailConfirmationToken, CancellationToken cancellationToken = default);
    Task<bool> RevokeAsync(EmailConfirmationToken emailConfirmationToken, CancellationToken cancellationToken = default);
}