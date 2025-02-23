using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using OrderTrak.Client.Provider;
using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IClient _apiService;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(IClient client, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
        {
            _apiService = client;
            _localStorageService = localStorageService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task Login(LoginDTO loginRequest)
        {
            var returnObj = await _apiService.LoginAsync(loginRequest);

            // Set String
            await _localStorageService.SetItemAsStringAsync("token", returnObj.Token);
            await _localStorageService.SetItemAsync("tokenExpiration", returnObj.Expiration);

            ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(returnObj.Token);
        }

        public async Task Register(RegisterDTO registerRequest)
        {
            await _apiService.RegisterAsync(registerRequest);
        }
    }
}
