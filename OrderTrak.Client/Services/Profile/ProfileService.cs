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

        public async Task UpdateUserAdminAsync(UserAdminUpdateDTO userAdminUpdateDTO)
        {
            await ApiService.UpdateUserAdminAsync(userAdminUpdateDTO);
        }

        public async Task<ProfileDTO> GetUserProfileAsync(Guid FormID)
        {
            return await ApiService.GetUserProfileByIDAsync(FormID);
        }

        public async Task DeleteUserAdminAsync(Guid FormID)
        {
            await ApiService.DeleteUserAdminAsync(FormID);
        }
    }
}
