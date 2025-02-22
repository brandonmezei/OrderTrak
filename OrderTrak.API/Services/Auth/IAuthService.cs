using OrderTrak.API.Models.DTO.Auth;

namespace OrderTrak.API.Services.Auth
{
    public interface IAuthService
    {
        Task Register(RegisterDTO registerDTO);
        Task<AuthReturnDTO> Login(LoginDTO loginDTO);
    }
}
