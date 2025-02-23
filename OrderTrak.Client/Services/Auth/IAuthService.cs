using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Auth
{
    public interface IAuthService
    {
        Task Login(LoginDTO loginRequest);
    }
}
