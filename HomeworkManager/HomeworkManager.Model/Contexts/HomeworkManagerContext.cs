using HomeworkManager.Model.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.Model.Contexts;

public class HomeworkManagerContext : IdentityDbContext<User, Role, Guid>
{
    public DbSet<AccessToken> AccessTokens => Set<AccessToken>();
    public DbSet<EmailConfirmationToken> EmailConfirmationTokens => Set<EmailConfirmationToken>();
    public DbSet<Entity> Entities => Set<Entity>();
    //public DbSet<PasswordRecoveryToken> PasswordRecoveryTokens => Set<PasswordRecoveryToken>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public HomeworkManagerContext(DbContextOptions<HomeworkManagerContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}