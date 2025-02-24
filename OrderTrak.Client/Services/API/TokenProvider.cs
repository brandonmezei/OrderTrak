using Blazored.LocalStorage;

namespace OrderTrak.Client.Services.API
{
    public class TokenProvider(ILocalStorageService localStorageService) : ITokenProvider
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;

        public async Task<string> GetTokenAsync()
        {
            return await _localStorageService.GetItemAsync<string>("token") ?? string.Empty;
        }
    }
}
