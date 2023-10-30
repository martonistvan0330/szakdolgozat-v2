using HomeworkManager.API.Hosting;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.CreateConfigurations();

var connectionString =
    builder.Configuration["PRODUCTION_DB_CONNECTION_STRING"]
    ?? builder.Configuration.GetConnectionString(nameof(HomeworkManagerContext));

builder.Services.AddDbContext<HomeworkManagerContext>(
    o => o.UseSqlServer(connectionString)
);

builder.Services.AddIdentityCore<User>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.User.RequireUniqueEmail = true;
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

builder.Services
    .AddRepositories()
    .AddServices()
    .AddManagers()
    .AddControllers();

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

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();