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
using OrderTrak.API.Services.Filters;
using OrderTrak.API.Services.Location;
using OrderTrak.API.Services.Parts;
using OrderTrak.API.Services.PO;
using OrderTrak.API.Services.Profile;
using OrderTrak.API.Services.Project;
using OrderTrak.API.Services.Receiving;
using OrderTrak.API.Services.Roles;
using OrderTrak.API.Services.StockGroup;
using OrderTrak.API.Services.StringHandlers;
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
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IRoleServices, RoleServices>();
builder.Services.AddScoped<IDropDownFilterFactory, DropDownFilterFactory>();
builder.Services.AddScoped<IPartService, PartService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IStockGroupService, StockGroupService>();
builder.Services.AddScoped<IPOService, POService>();
builder.Services.AddScoped<IReceivingService, ReceivingService>();


// Register custom authorization handler
builder.Services.AddScoped<IAuthorizationHandler, FunctionAccessHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Customer", policy =>
        policy.Requirements.Add(new FunctionAccessRequirement("Customer")));

    options.AddPolicy("Project", policy =>
        policy.Requirements.Add(new FunctionAccessRequirement("Project")));

    options.AddPolicy("Role", policy =>
        policy.Requirements.Add(new FunctionAccessRequirement("Role")));

    options.AddPolicy("UserManager", policy =>
        policy.Requirements.Add(new FunctionAccessRequirement("UserManager")));

    options.AddPolicy("Parts", policy =>
        policy.Requirements.Add(new FunctionAccessRequirement("Parts")));

    options.AddPolicy("Location", policy =>
        policy.Requirements.Add(new FunctionAccessRequirement("Location")));

    options.AddPolicy("StockGroup", policy =>
        policy.Requirements.Add(new FunctionAccessRequirement("StockGroup")));

    options.AddPolicy("PurchaseOrder", policy =>
        policy.Requirements.Add(new FunctionAccessRequirement("PurchaseOrder")));

    options.AddPolicy("Receiving", policy =>
        policy.Requirements.Add(new FunctionAccessRequirement("Receiving")));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", builder =>
    {
        builder.WithOrigins("https://localhost:7049")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowBlazorClient");

app.UseAuthentication();
app.UseAuthorization();

app.UseUsernameMiddleware();

app.MapControllers();

app.Run();
