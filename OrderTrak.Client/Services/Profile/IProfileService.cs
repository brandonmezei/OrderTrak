using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Profile
{
    public interface IProfileService
    {
        Task UpdateProfileAsync(ProfileUpdateDTO profileUpdateDTO);
        Task<ProfileDTO> GetUserProfileAsync();
        Task<ProfileDTO> GetUserProfileAsync(Guid FormID);
        Task<PagedTableOfProfileDTO> SearchUserProfileAsync(SearchQueryDTO searchQuery);
        Task UpdateUserAdminAsync(UserAdminUpdateDTO userAdminUpdateDTO);
        Task DeleteUserAdminAsync(Guid FormID);
    }
}
