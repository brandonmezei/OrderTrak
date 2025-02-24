using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using OrderTrak.Client.Provider;
using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Auth
{
    public class AuthService(IClient client, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider) : IAuthService
    {
        private readonly IClient _apiService = client;
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;

        public async Task Login(LoginDTO loginRequest)
        {
            var returnObj = await _apiService.LoginAsync(loginRequest);

            // Set String
            await _localStorageService.SetItemAsStringAsync("token", returnObj.Token);
            await _localStorageService.SetItemAsync("tokenExpiration", returnObj.Expiration);
            await _localStorageService.SetItemAsync("fullname", returnObj.FullName);

            var permissionList = await _apiService.PermissionsAsync();

            // Set Permissions
            await _localStorageService.SetItemAsync("permissions", permissionList.ToList());

            ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(returnObj.Token);
        }

        public async Task Register(RegisterDTO registerRequest)
        {
            await _apiService.RegisterAsync(registerRequest);
        }
    }
}
