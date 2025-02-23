using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OrderTrak.Client;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Auth;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddTransient<TokenHttpClientHandler>();

builder.Services.AddBlazoredLocalStorage();

var baseAddress = builder.Configuration.GetValue<string>("API:BaseUrl") ?? throw new Exception("No API defined.");

builder.Services.AddHttpClient<IClient, Client>(client =>
{
    client.BaseAddress = new Uri(baseAddress);
})
    .AddHttpMessageHandler<TokenHttpClientHandler>();


await builder.Build().RunAsync();
