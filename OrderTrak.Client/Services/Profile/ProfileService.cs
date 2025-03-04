using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Profile
{
    public class ProfileService(IClient client) : IProfileService
    {
        private readonly IClient ApiService = client;

        public async Task<ProfileDTO> GetUserProfileAsync()
        {
            return await ApiService.GetUserProfileAsync();
        }

        public async Task UpdateProfileAsync(ProfileUpdateDTO profileUpdateDTO)
        {
            await ApiService.UpdateProfileAsync(profileUpdateDTO);
        }

        public async Task<PagedTableOfProfileDTO> SearchUserProfileAsync(SearchQueryDTO searchQuery)
        {
            return await ApiService.SearchUserProfileAsync(searchQuery);
        }
    }
}
