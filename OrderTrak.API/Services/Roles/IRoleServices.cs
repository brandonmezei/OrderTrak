using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Roles;

namespace OrderTrak.API.Services.Roles
{
    public interface IRoleServices
    {
        Task<Guid> CreateRoleAsync(RoleCreateDTO roleCreateDTO);
        Task UpdateRoleAsync(RoleUpdateDTO roleUpdateDTO);
        Task DeleteRoleAsync(Guid roleID);
        Task<RoleDTO> GetRoleAsync(Guid roleID);
        Task<PagedTable<RoleSearchReturnDTO>> SearchRolesAsync(RoleSearchDTO searchQuery);
        Task<List<RoleToFunctionDTO>> GetRoleToFunctionByRoleIDAsync(Guid roleID);
        Task UpdateRoleToFunctionAsync(RoleUpdateRoleToFunctionDTO roleToFunctionUpdateDTO);
    }
}
