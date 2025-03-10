using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Filters
{
    public class DropDownFactoryService(IClient client) : IDropDownFactoryService
    {
        private readonly IClient ApiService = client;

        public async Task<List<DropDownFilterDTO>> GetUnassignedUsers()
        {
            return [.. await ApiService.GetUnassignedUsersDropDownAsync()];
        }

        public async Task<List<DropDownFilterDTO>> GetUOMAsync()
        {
            return [.. await ApiService.GetUOMDropDownAsync()];
        }
    }
}
