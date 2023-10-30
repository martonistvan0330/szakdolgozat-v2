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

    public async Task<EmailConfirmationToken?> GetActiveByUserAsync(Guid userId)
    {
        return await _context.EmailConfirmationTokens
            .Where(ect => ect.UserId == userId && ect.IsActive)
            .SingleOrDefaultAsync();
    }

    public async Task<string?> CreateAsync(Guid userId, string emailConfirmationToken)
    {
        var dbEmailConfirmationToken = await GetActiveByUserAsync(userId);

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
        await _context.SaveChangesAsync();

        return emailConfirmationToken;
    }

    public async Task RevokeAsync(EmailConfirmationToken emailConfirmationToken)
    {
        emailConfirmationToken.IsActive = false;
        await _context.SaveChangesAsync();
    }
}