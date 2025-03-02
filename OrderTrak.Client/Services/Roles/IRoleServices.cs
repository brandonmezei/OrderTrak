using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Roles
{
    public interface IRoleServices
    {
        Task<Guid> CreateRoleAsync(RoleCreateDTO roleCreateDTO);
        Task UpdateRoleAsync(RoleUpdateDTO roleUpdateDTO);
        Task DeleteRoleAsync(Guid roleID);
        Task<RoleDTO> GetRoleAsync(Guid roleID);
        Task<PagedTableOfRoleSearchReturnDTO> SearchRolesAsync(RoleSearchDTO searchQuery);
        Task<List<RoleToFunctionDTO>> GetRoleToFunctionByRoleIDAsync(Guid roleID);
        Task UpdateRoleToFunctionAsync(RoleUpdateRoleToFunctionDTO roleToFunctionUpdateDTO);
        Task<PagedTableOfRoleToUserReturnDTO> GetUserByRolesAsync(RoleToUserSearchDTO searchQuery);
        Task DeleteUserFromRoleAsync(RoleToUserSelectDTO deleteDTO);
        Task AddUserToRoleAsync(RoleToUserSelectDTO addDTO);
    }
}
