using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.Entities;
using HomeworkManager.Shared.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.DataAccess.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly HomeworkManagerContext _context;
    private readonly IHashingService _hashingService;

    public RefreshTokenRepository(HomeworkManagerContext context, IHashingService hashingService)
    {
        _context = context;
        _hashingService = hashingService;
    }

    public async Task<RefreshToken?> GetAsync(string refreshToken, AccessToken accessToken)
    {
        string refreshTokenHash = await _hashingService.GetHashString(refreshToken);
        
        return await _context.RefreshTokens
            .Where(rt => rt.TokenHash == refreshTokenHash
                         && rt.AccessTokenId == accessToken.AccessTokenId)
            .SingleOrDefaultAsync();
    }

    public async Task RevokeAsync(string refreshToken, AccessToken accessToken)
    {
        var dbRefreshToken = await GetAsync(refreshToken, accessToken);

        if (dbRefreshToken is not null)
        {
            dbRefreshToken.IsActive = false;
        }

        await _context.SaveChangesAsync();
    }
}