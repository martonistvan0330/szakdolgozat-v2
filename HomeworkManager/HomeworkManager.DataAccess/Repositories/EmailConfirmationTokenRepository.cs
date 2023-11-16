using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.DataAccess.Repositories;

public class EmailConfirmationTokenRepository : IEmailConfirmationTokenRepository
{
    private readonly HomeworkManagerContext _context;

    public EmailConfirmationTokenRepository(HomeworkManagerContext context)
    {
        _context = context;
    }

    public async Task<EmailConfirmationToken?> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.EmailConfirmationTokens
            .Where(ect => ect.UserId == userId && ect.IsActive)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsActiveAsync(string emailConfirmationToken, CancellationToken cancellationToken = default)
    {
        return await _context.EmailConfirmationTokens
            .Where(ect => ect.Token == emailConfirmationToken && ect.IsActive)
            .AnyAsync(cancellationToken);
    }

    public async Task<string> CreateAsync(Guid userId, string emailConfirmationToken, CancellationToken cancellationToken = default)
    {
        var dbEmailConfirmationToken = await GetActiveByUserIdAsync(userId, cancellationToken);

        if (dbEmailConfirmationToken is not null)
        {
            dbEmailConfirmationToken.IsActive = false;
        }

        var newEmailConfirmationToken = new EmailConfirmationToken
        {
            Token = emailConfirmationToken,
            UserId = userId
        };

        _context.EmailConfirmationTokens.Add(newEmailConfirmationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return emailConfirmationToken;
    }

    public async Task<bool> RevokeAsync(EmailConfirmationToken emailConfirmationToken, CancellationToken cancellationToken = default)
    {
        emailConfirmationToken.IsActive = false;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}