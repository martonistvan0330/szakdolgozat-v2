using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.DataAccess.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly HomeworkManagerContext _context;

        public RefreshTokenRepository(HomeworkManagerContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetAsync(AccessToken accessToken, string refreshToken)
        {
            return await _context.RefreshTokens
                .Where(rt => rt.Token == refreshToken
                             && rt.AccessTokenId == accessToken.AccessTokenId)
                .SingleOrDefaultAsync();
        }

        public async Task RevokeAsync(AccessToken accessToken, string refreshToken)
        {
            var dbRefreshToken = await GetAsync(accessToken, refreshToken);

            if (dbRefreshToken is not null)
            {
                dbRefreshToken.IsActive = false;
            }

            await _context.SaveChangesAsync();
        }
    }
}