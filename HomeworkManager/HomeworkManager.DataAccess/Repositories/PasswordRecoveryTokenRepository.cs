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

    public async Task<string?> GetUserIdByActiveTokenAsync(string passwordRecoveryToken)
    {
        return await _context.PasswordRecoveryTokens
            .Where(prt => prt.Token == passwordRecoveryToken && prt.IsActive)
            .Select(prt => prt.UserId.ToString())
            .SingleOrDefaultAsync();
    }

    public async Task<string?> CreateAsync(Guid userId, string passwordRecoveryToken)
    {
        var dbPasswordRecoveryToken = await GetActiveByUserAsync(userId);

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
        await _context.SaveChangesAsync();

        return passwordRecoveryToken;
    }
    
    public async Task RevokeAsync(string passwordRecoveryToken)
    {
        var dbPasswordRecoveryToken = await _context.PasswordRecoveryTokens
            .SingleOrDefaultAsync(prt => prt.Token == passwordRecoveryToken);

        if (dbPasswordRecoveryToken is not null)
        {
            dbPasswordRecoveryToken.IsActive = false;
            await _context.SaveChangesAsync();            
        }
    }
    
    private async Task<PasswordRecoveryToken?> GetActiveByUserAsync(Guid userId)
    {
        return await _context.PasswordRecoveryTokens
            .Where(prt => prt.UserId == userId && prt.IsActive)
            .SingleOrDefaultAsync();
    }
}