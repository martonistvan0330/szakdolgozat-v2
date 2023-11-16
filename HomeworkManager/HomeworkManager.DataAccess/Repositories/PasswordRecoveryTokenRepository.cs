using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.DataAccess.Repositories;

public class PasswordRecoveryTokenRepository : IPasswordRecoveryTokenRepository
{
    private readonly HomeworkManagerContext _context;

    public PasswordRecoveryTokenRepository(HomeworkManagerContext context)
    {
        _context = context;
    }

    public async Task<Guid?> GetUserIdByActiveTokenAsync(string passwordRecoveryToken, CancellationToken cancellationToken = default)
    {
        return await _context.PasswordRecoveryTokens
            .Where(prt => prt.Token == passwordRecoveryToken && prt.IsActive)
            .Select(prt => prt.UserId)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<PasswordRecoveryToken?> CreateAsync(Guid userId, string passwordRecoveryToken, CancellationToken cancellationToken = default)
    {
        var dbPasswordRecoveryToken = await GetActiveByUserAsync(userId, cancellationToken);

        if (dbPasswordRecoveryToken is not null)
        {
            dbPasswordRecoveryToken.IsActive = false;
        }

        var newPasswordRecoveryToken = new PasswordRecoveryToken
        {
            Token = passwordRecoveryToken,
            UserId = userId
        };

        _context.PasswordRecoveryTokens.Add(newPasswordRecoveryToken);
        await _context.SaveChangesAsync(cancellationToken);

        return newPasswordRecoveryToken;
    }

    public async Task RevokeAsync(string passwordRecoveryToken, CancellationToken cancellationToken = default)
    {
        var dbPasswordRecoveryToken = await _context.PasswordRecoveryTokens
            .Where(prt => prt.Token == passwordRecoveryToken)
            .SingleOrDefaultAsync(cancellationToken);

        if (dbPasswordRecoveryToken is not null)
        {
            dbPasswordRecoveryToken.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task<PasswordRecoveryToken?> GetActiveByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.PasswordRecoveryTokens
            .Where(prt => prt.UserId == userId && prt.IsActive)
            .SingleOrDefaultAsync(cancellationToken);
    }
}