using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Services.Auth;
using OrderTrak.API.Models.OrderTrakDB;
using System.Collections.ObjectModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure Serilog
// dotnet user-secrets init
// dotnet user-secrets set "ConnectionStrings:OrderTrakDatabase" "Server=BRANDON-PC;Database=OrderTrak;User Id=OrderTrak;Password=Mezei3657!;Trusted_Connection=True;Encrypt=false"
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
#if DEBUG
    .AddUserSecrets<Program>()
#else
    .AddEnvironmentVariables()
#endif
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Register DbContext with dependency injection
builder.Services.AddDbContext<OrderTrakContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("OrderTrakDatabase")));

// Register services
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
