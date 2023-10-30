using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IEmailConfirmationTokenRepository
{
    Task<EmailConfirmationToken?> GetActiveByUserAsync(Guid userId);
    Task<string?> CreateAsync(Guid userId, string emailConfirmationToken);
    Task RevokeAsync(EmailConfirmationToken emailConfirmationToken);
}