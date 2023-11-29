using HomeworkManager.API.Hosting;
using HomeworkManager.API.Hubs;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.CreateConfigurations();

string? connectionString =
    builder.Configuration["PRODUCTION_DB_CONNECTION_STRING"]
    ?? builder.Configuration.GetConnectionString(nameof(HomeworkManagerContext));

builder.Services.AddDbContext<HomeworkManagerContext>(
    o => o.UseSqlServer(connectionString)
);

builder.Services.AddIdentityCore<User>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.User.RequireUniqueEmail = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<Role>()
    .AddEntityFrameworkStores<HomeworkManagerContext>()
    .AddDefaultTokenProviders();

builder.AddJwtAuthentication();

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddRepositories()
    .AddServices()
    .AddManagers()
    .AddValidators()
    .AddControllers();

builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await app.MigrateDatabase<HomeworkManagerContext>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<AppointmentHub>("/appointment");

app.Run();