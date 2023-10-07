using HomeworkManager.API.Hosting;
using HomeworkManager.Model.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("PRODUCTION_DB_CONNECTION_STRING");

if (connectionString is null)
{
    connectionString = builder.Configuration.GetConnectionString(nameof(HomeworkManagerContext));
}

// Add services to the container.

builder.Services.AddDbContext<HomeworkManagerContext>(
    o => o.UseSqlServer(connectionString)
);

builder.Services.AddRepositories();
builder.Services.AddManagers();

builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
