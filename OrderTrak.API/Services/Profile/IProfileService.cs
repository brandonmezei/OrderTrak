using OrderTrak.API.Models.DTO.Profile;

namespace OrderTrak.API.Services.Profile
{
    public interface IProfileService
    {
        Task UpdateProfileAsync(ProfileUpdateDTO profileUpdateDTO);
        Task<ProfileDTO> GetUserProfileAsync();
    }
}
