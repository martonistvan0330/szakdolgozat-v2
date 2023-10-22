using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.Entities;
using HomeworkManager.Shared.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.DataAccess.Repositories;

public class AccessTokenRepository : IAccessTokenRepository
{
    private readonly HomeworkManagerContext _context;
    private readonly IHashingService _hashingService;

    public AccessTokenRepository(HomeworkManagerContext context, IHashingService hashingService)
    {
        _context = context;
        _hashingService = hashingService;
    }

    public async Task<AccessToken?> GetAsync(string accessToken, Guid userId)
    {
        var accessTokenHash = await _hashingService.GetHashString(accessToken);

        return await _context.AccessTokens
            .Where(rt => rt.TokenHash == accessTokenHash
                         && rt.UserId == userId)
            .SingleOrDefaultAsync();
    }

    public async Task CreateAsync(string accessToken, string refreshToken, Guid userId)
    {
        var accessTokenHash = await _hashingService.GetHashString(accessToken);
        var refreshTokenHash = await _hashingService.GetHashString(refreshToken);

        AccessToken dbAccessToken = new()
        {
            TokenHash = accessTokenHash,
            UserId = userId
        };

        dbAccessToken.RefreshToken = new RefreshToken
        {
            TokenHash = refreshTokenHash,
            AccessToken = dbAccessToken
        };

        _context.AccessTokens.Add(dbAccessToken);
        await _context.SaveChangesAsync();
    }

    public async Task<AccessToken?> RevokeAsync(string accessToken, Guid userId)
    {
        var dbAccessToken = await GetAsync(accessToken, userId);

        if (dbAccessToken is not null)
        {
            dbAccessToken.IsActive = false;
        }

        await _context.SaveChangesAsync();

        return dbAccessToken;
    }
}