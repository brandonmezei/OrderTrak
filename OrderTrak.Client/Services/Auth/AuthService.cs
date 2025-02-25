using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using OrderTrak.Client.Provider;
using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Auth
{
    public class AuthService(IClient client, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider) : IAuthService
    {
        private readonly IClient ApiService = client;
        private readonly ILocalStorageService LocalStorageService = localStorageService;
        private readonly AuthenticationStateProvider AuthenticationStateProvider = authenticationStateProvider;

        public async Task Login(LoginDTO loginRequest)
        {
            var returnObj = await ApiService.LoginAsync(loginRequest);

            // Set String
            await LocalStorageService.SetItemAsStringAsync("token", returnObj.Token);
            await LocalStorageService.SetItemAsync("tokenExpiration", returnObj.Expiration);
            await LocalStorageService.SetItemAsync("fullname", returnObj.FullName);

            var permissionList = await ApiService.PermissionsAsync();

            // Set Permissions
            await LocalStorageService.SetItemAsync("permissions", permissionList.ToList());

            ((CustomAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsAuthenticated(returnObj.Token);
        }

        public async Task Register(RegisterDTO registerRequest)
        {
            await ApiService.RegisterAsync(registerRequest);
        }
    }
}
