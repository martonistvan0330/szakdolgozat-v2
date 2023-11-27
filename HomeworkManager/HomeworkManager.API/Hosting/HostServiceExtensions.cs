using FluentValidation;
using HomeworkManager.API.Validation;
using HomeworkManager.API.Validation.Appointment;
using HomeworkManager.API.Validation.Assignment;
using HomeworkManager.API.Validation.Authentication;
using HomeworkManager.API.Validation.Course;
using HomeworkManager.API.Validation.Group;
using HomeworkManager.API.Validation.Role;
using HomeworkManager.API.Validation.Submission;
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
using HomeworkManager.Model.CustomEntities.Appointment;
using HomeworkManager.Model.CustomEntities.Assignment;
using HomeworkManager.Model.CustomEntities.Authentication;
using HomeworkManager.Model.CustomEntities.Course;
using HomeworkManager.Model.CustomEntities.Group;
using HomeworkManager.Model.CustomEntities.Submission;
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
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IEmailConfirmationTokenRepository, EmailConfirmationTokenRepository>();
        services.AddScoped<IEntityRepository, EntityRepository>();
        services.AddScoped<IPasswordRecoveryTokenRepository, PasswordRecoveryTokenRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ISubmissionRepository, SubmissionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }

    public static IServiceCollection AddManagers(this IServiceCollection services)
    {
        services.AddScoped<IAppointmentManager, AppointmentManager>();
        services.AddScoped<IAssignmentManager, AssignmentManager>();
        services.AddScoped<IAuthenticationManager, AuthenticationManager>();
        services.AddScoped<ICourseManager, CourseManager>();
        services.AddScoped<IEntityManager, EntityManager>();
        services.AddScoped<IGroupManager, GroupManager>();
        services.AddScoped<IRoleManager, RoleManager>();
        services.AddScoped<ISubmissionManager, SubmissionManager>();
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
        services.AddScoped<IValidator<EmailConfirmationRequest>, EmailConfirmationRequestValidator>();
        services.AddScoped<IValidator<GroupInfo>, GroupNameValidator>();
        services.AddScoped<IValidator<NewAppointment>, NewAppointmentValidator>();
        services.AddScoped<IValidator<NewAssignment>, NewAssignmentValidator>();
        services.AddScoped<IValidator<NewCourse>, NewCourseValidator>();
        services.AddScoped<IValidator<NewGroup>, NewGroupValidator>();
        services.AddScoped<IValidator<NewUser>, NewUserValidator>();
        services.AddScoped<IValidator<PasswordResetRequest>, PasswordResetRequestValidator>();
        services.AddScoped<IValidator<UpdatedAssignment>, UpdatedAssignmentValidator>();
        services.AddScoped<IValidator<UpdatedCourse>, UpdatedCourseValidator>();
        services.AddScoped<IValidator<UpdatedGroup>, UpdatedGroupValidator>();
        services.AddScoped<IValidator<UpdatedTextSubmission>, UpdatedTextSubmissionValidator>();

        services.AddScoped<AssignmentIdValidator>();
        services.AddScoped<CourseIdValidator>();
        services.AddScoped<EmailValidator>();
        services.AddScoped<PasswordValidator>();
        services.AddScoped<RoleValidator>();
        services.AddScoped<SubmissionIdValidator>();
        services.AddScoped<UserIdValidator>();
        return services;
    }
}