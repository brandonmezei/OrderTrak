using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Roles
{
    public class RoleServices(IClient client) : IRoleServices
    {
        private readonly IClient ApiService = client;

        public async Task<Guid> CreateRoleAsync(RoleCreateDTO roleCreateDTO)
        {
            return await ApiService.CreateRoleAsync(roleCreateDTO);
        }

        public async Task DeleteRoleAsync(Guid roleID)
        {
            await ApiService.DeleteRoleAsync(roleID);
        }

        public async Task<RoleDTO> GetRoleAsync(Guid roleID)
        {
            return await ApiService.GetRoleAsync(roleID);
        }

        public async Task<PagedTableOfRoleSearchReturnDTO> SearchRolesAsync(RoleSearchDTO searchQuery)
        {
            return await ApiService.SearchRolesAsync(searchQuery);
        }

        public async Task UpdateRoleAsync(RoleUpdateDTO roleUpdateDTO)
        {
            await ApiService.UpdateRoleAsync(roleUpdateDTO);
        }

        public async Task<List<RoleToFunctionDTO>> GetRoleToFunctionByRoleIDAsync(Guid roleID)
        {
            return [.. await ApiService.GetRoleToFunctionByRoleIDAsync(roleID)];
        }

        public async Task UpdateRoleToFunctionAsync(RoleUpdateRoleToFunctionDTO roleToFunctionUpdateDTO)
        {
            await ApiService.UpdateRoleToFunctionAsync(roleToFunctionUpdateDTO);
        }

        public async Task<PagedTableOfRoleToUserReturnDTO> GetUserByRolesAsync(RoleToUserSearchDTO searchQuery)
        {
            return await ApiService.SearchUserByRolesAsync(searchQuery);
        }

        public async Task DeleteUserFromRoleAsync(RoleToUserSelectDTO deleteDTO)
        {
            await ApiService.DeleteUserFromRoleAsync(deleteDTO);
        }

        public async Task AddUserToRoleAsync(RoleToUserSelectDTO addDTO)
        {
            await ApiService.AddUserToRoleAsync(addDTO);
        }
    }
}
