using AutoMapper;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OrderTrak.Client;
using OrderTrak.Client.Models;
using OrderTrak.Client.Provider;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Auth;
using OrderTrak.Client.Services.ChangeLog;
using OrderTrak.Client.Services.Customer;
using OrderTrak.Client.Services.Filters;
using OrderTrak.Client.Services.Parts;
using OrderTrak.Client.Services.Profile;
using OrderTrak.Client.Services.Project;
using OrderTrak.Client.Services.Roles;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add AutoMapper
// Auto Mapper Configurations
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// Scope Services
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IChangeLogService, ChangeLogService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IRoleServices, RoleServices>();
builder.Services.AddScoped<IDropDownFactoryService, DropDownFactoryService>();
builder.Services.AddScoped<IPartService, PartService>();

builder.Services.AddTransient<TokenHttpClientHandler>();

builder.Services.AddBlazoredLocalStorage();

var baseAddress = builder.Configuration.GetValue<string>("API:BaseUrl") ?? throw new Exception("No API defined.");

builder.Services.AddHttpClient<IClient, Client>(client =>
{
    client.BaseAddress = new Uri(baseAddress);
})
    .AddHttpMessageHandler<TokenHttpClientHandler>();

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
