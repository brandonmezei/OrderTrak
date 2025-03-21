﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OrderTrak.Client.Provider
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService LocalStorageService;

        public CustomAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            LocalStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await LocalStorageService.GetItemAsync<string>("token") ?? string.Empty;
            var expiration = await LocalStorageService.GetItemAsync<DateTimeOffset>("tokenExpiration");

            if (string.IsNullOrEmpty(token) || expiration <= DateTime.UtcNow)
            {
                MarkUserAsLoggedOut();
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var claims = jwtToken.Claims.ToList();

            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        public void MarkUserAsAuthenticated(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var claims = jwtToken.Claims.ToList();

            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void MarkUserAsLoggedOut()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);

            // Remove Local Storage Items
            LocalStorageService.RemoveItemAsync("token");
            LocalStorageService.RemoveItemAsync("tokenExpiration");
            LocalStorageService.RemoveItemAsync("fullname");
            LocalStorageService.RemoveItemAsync("permissions");


            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
