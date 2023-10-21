using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.DataAccess.Repositories;

public class AccessTokenRepository : IAccessTokenRepository
{
    private readonly HomeworkManagerContext _context;

    public AccessTokenRepository(HomeworkManagerContext context)
    {
        _context = context;
    }

    public async Task<AccessToken?> GetAsync(string accessToken, Guid userId)
    {
        return await _context.AccessTokens
            .Where(rt => rt.Token == accessToken
                         && rt.UserId == userId)
            .SingleOrDefaultAsync();
    }

    public async Task CreateAsync(string accessToken, string refreshToken, Guid userId)
    {
        AccessToken dbAccessToken = new()
        {
            Token = accessToken,
            UserId = userId
        };

        dbAccessToken.RefreshToken = new RefreshToken
        {
            Token = refreshToken,
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