using HomeworkManager.Model.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.Model.Contexts
{
    public class HomeworkManagerContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Entity> Entities => Set<Entity>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        public HomeworkManagerContext(DbContextOptions<HomeworkManagerContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }
    }
}