using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IClient _apiService;

        public AuthService(IClient client)
        {
            _apiService = client;
        }

        public async Task Login(LoginDTO loginRequest)
        {
            var returnObj = await _apiService.LoginAsync(loginRequest);
        }
    }
}
