using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderTrak.API.Models.OrderTrakDB;
using OrderTrak.API.Providers;
using OrderTrak.API.Providers.AuthRequirements;
using OrderTrak.API.Services.Auth;
using OrderTrak.API.Services.ChangeLog;
using OrderTrak.API.Services.Customer;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new TrimmingJsonConverter());
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure JWT authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new Exception("No JWT Key"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Configure Serilog
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

builder.Services.AddHttpContextAccessor();

// Register services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IChangeLogService, ChangeLogService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// Register custom authorization handler
builder.Services.AddScoped<IAuthorizationHandler, FunctionAccessHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Customer", policy =>
        policy.Requirements.Add(new FunctionAccessRequirement("Customer")));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseUsernameMiddleware();

app.MapControllers();

app.Run();
