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

    public async Task<AccessToken?> GetAsync(User user, string accessToken)
    {
        return await _context.AccessTokens
            .Where(rt => rt.Token == accessToken
                         && rt.UserId == user.Id)
            .SingleOrDefaultAsync();
    }

    public async Task<AccessToken?> RevokeAsync(User user, string accessToken)
    {
        var dbAccessToken = await GetAsync(user, accessToken);

        if (dbAccessToken is not null)
        {
            dbAccessToken.IsActive = false;
        }

        await _context.SaveChangesAsync();

        return dbAccessToken;
    }
}