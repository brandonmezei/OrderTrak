using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Profile
{
    public interface IProfileService
    {
        Task UpdateProfileAsync(ProfileUpdateDTO profileUpdateDTO);
        Task<ProfileDTO> GetUserProfileAsync();
        Task<PagedTableOfProfileDTO> SearchUserProfileAsync(SearchQueryDTO searchQuery);
    }
}
