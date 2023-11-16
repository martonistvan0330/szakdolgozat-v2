using FluentValidation;
using HomeworkManager.API.Validation;
using HomeworkManager.API.Validation.Authentication;
using HomeworkManager.API.Validation.Course;
using HomeworkManager.API.Validation.Group;
using HomeworkManager.API.Validation.Role;
using HomeworkManager.API.Validation.User;
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
using HomeworkManager.Model.CustomEntities.Authentication;
using HomeworkManager.Model.CustomEntities.Course;
using HomeworkManager.Model.CustomEntities.Group;
using HomeworkManager.Model.CustomEntities.User;
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
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IEmailConfirmationTokenRepository, EmailConfirmationTokenRepository>();
        services.AddScoped<IEntityRepository, EntityRepository>();
        services.AddScoped<IPasswordRecoveryTokenRepository, PasswordRecoveryTokenRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }

    public static IServiceCollection AddManagers(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationManager, AuthenticationManager>();
        services.AddScoped<ICourseManager, CourseManager>();
        services.AddScoped<IEntityManager, EntityManager>();
        services.AddScoped<IGroupManager, GroupManager>();
        services.AddScoped<IRoleManager, RoleManager>();
        services.AddScoped<IUserManager, UserManager>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<ICurrentUserService, CurrentUserService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IHashingService, HashingService>();
        services.AddTransient<IJwtService, JwtService>();
        services.AddTransient<IRoleSeedService, RoleSeedService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IUserSeedService, UserSeedService>();
        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<AbstractValidator<EmailConfirmationRequest>, EmailConfirmationRequestValidator>();
        services.AddScoped<AbstractValidator<NewCourse>, NewCourseValidator>();
        services.AddScoped<AbstractValidator<NewUser>, NewUserValidator>();
        services.AddScoped<AbstractValidator<PasswordResetRequest>, PasswordResetRequestValidator>();

        services.AddScoped<CourseIdValidator>();
        services.AddScoped<EmailValidator>();
        services.AddScoped<PasswordValidator>();
        services.AddScoped<RoleValidator>();
        services.AddScoped<UserIdValidator>();
        return services;
    }
}