using HomeworkManager.BusinessLogic.Services.Seed.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.API.Hosting
{
    public static class HostDataExtensions
    {
        public static async Task<IHost> MigrateDatabase<TContext>(this IHost host) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();

            var serviceProvider = scope.ServiceProvider;

            var context = serviceProvider.GetRequiredService<TContext>();
            await context.Database.MigrateAsync();

            await serviceProvider.GetRequiredService<IRoleSeedService>().SeedRoleAsync();
            await serviceProvider.GetRequiredService<IUserSeedService>().SeedUserAsync();

            return host;
        }
    }
}