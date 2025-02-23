using Blazored.LocalStorage;

namespace OrderTrak.Client.Services.API
{
    public class TokenProvider : ITokenProvider
    {
        private readonly ILocalStorageService _localStorageService;

        public TokenProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<string> GetTokenAsync()
        {
            return await _localStorageService.GetItemAsync<string>("token") ?? string.Empty;
        }
    }
}
