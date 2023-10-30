using HomeworkManager.BusinessLogic.Managers;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.BusinessLogic.Services.Authentication;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.BusinessLogic.Services.Email;
using HomeworkManager.BusinessLogic.Services.Email.Interfaces;
using HomeworkManager.BusinessLogic.Services.Seed;
using HomeworkManager.BusinessLogic.Services.Seed.Interfaces;
using HomeworkManager.DataAccess.Repositories;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Configurations;
using HomeworkManager.Shared.Services;
using HomeworkManager.Shared.Services.Interfaces;

namespace HomeworkManager.API.Hosting;

public static class HostServiceExtensions
{
    public static IServiceCollection CreateConfigurations(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("JWT"));
        builder.Services.Configure<SmtpConfiguration>(builder.Configuration.GetSection("SMTP"));
        return builder.Services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAccessTokenRepository, AccessTokenRepository>();
        services.AddScoped<IEmailConfirmationTokenRepository, EmailConfirmationTokenRepository>();
        services.AddScoped<IEntityRepository, EntityRepository>();
        services.AddScoped<IPasswordRecoveryTokenRepository, PasswordRecoveryTokenRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }

    public static IServiceCollection AddManagers(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationManager, AuthenticationManager>();
        services.AddScoped<IEntityManager, EntityManager>();
        services.AddScoped<UserManager>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IHashingService, HashingService>();
        services.AddTransient<IJwtService, JwtService>();
        services.AddTransient<IRoleSeedService, RoleSeedService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IUserSeedService, UserSeedService>();
        return services;
    }
}