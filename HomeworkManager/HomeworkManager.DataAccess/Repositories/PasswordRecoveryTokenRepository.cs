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

    public async Task<PasswordRecoveryToken?> GetActiveByUserAsync(Guid userId)
    {
        return await _context.PasswordRecoveryTokens
            .Where(ect => ect.UserId == userId && ect.IsActive)
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

    public async Task RevokeAsync(PasswordRecoveryToken passwordRecoveryToken)
    {
        passwordRecoveryToken.IsActive = false;
        await _context.SaveChangesAsync();
    }
}