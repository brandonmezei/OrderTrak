using Blazored.LocalStorage;

namespace OrderTrak.Client.Services.API
{
    public class TokenProvider(ILocalStorageService localStorageService) : ITokenProvider
    {
        private readonly ILocalStorageService LocalStorageService = localStorageService;

        public async Task<string> GetTokenAsync()
        {
            return await LocalStorageService.GetItemAsync<string>("token") ?? string.Empty;
        }
    }
}
