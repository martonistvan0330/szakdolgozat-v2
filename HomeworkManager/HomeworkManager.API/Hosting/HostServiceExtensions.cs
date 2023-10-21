using HomeworkManager.BusinessLogic.Managers;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.BusinessLogic.Services.Authentication;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.BusinessLogic.Services.Seed;
using HomeworkManager.BusinessLogic.Services.Seed.Interfaces;
using HomeworkManager.DataAccess.Repositories;
using HomeworkManager.DataAccess.Repositories.Interfaces;

namespace HomeworkManager.API.Hosting
{
    public static class HostServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAccessTokenRepository, AccessTokenRepository>();
            services.AddScoped<IEntityRepository, EntityRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            return services;
        }

        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            services.AddScoped<IEntityManager, EntityManager>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<IRoleSeedService, RoleSeedService>();
            services.AddTransient<IUserSeedService, UserSeedService>();
            return services;
        }
    }
}