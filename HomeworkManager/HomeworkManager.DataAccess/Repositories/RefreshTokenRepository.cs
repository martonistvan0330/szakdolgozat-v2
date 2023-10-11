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

        public async Task<bool> UserHasTokenAsync(User user, string refreshToken)
        {
            return await _context.RefreshTokens
                .Where(rt => rt.UserId == user.Id)
                .Select(rt => rt.Token)
                .ContainsAsync(refreshToken);
        }
    }
}