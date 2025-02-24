using OrderTrak.API.Models.DTO.Auth;

namespace OrderTrak.API.Services.Auth
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDTO registerDTO);
        Task<AuthReturnDTO> LoginAsync(LoginDTO loginDTO);
        Task<List<string>> FetchPermissionsAsync();
    }
}
