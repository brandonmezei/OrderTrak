using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Roles;
using OrderTrak.API.Models.OrderTrakDB;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Services.Roles
{
    public class RoleServices(OrderTrakContext orderTrakContext) : IRoleServices
    {
        private readonly OrderTrakContext DB = orderTrakContext;

        public async Task<Guid> CreateRoleAsync(RoleCreateDTO roleCreateDTO)
        {
            // Check if role already exists
            if (await DB.SYS_Roles.AnyAsync(x => x.RoleName == roleCreateDTO.RoleName))
                throw new ValidationException($"Role {roleCreateDTO.RoleName} already exists.");

            // Create new Role
            var role = new SYS_Roles
            {
                RoleName = roleCreateDTO.RoleName ?? throw new ValidationException("Role Name is required.")
            };

            DB.SYS_Roles.Add(role);

            // Save
            await DB.SaveChangesAsync();

            return role.FormID;
        }

        public async Task UpdateRoleAsync(RoleUpdateDTO roleUpdateDTO)
        {
            // Get Role
            var role = await DB.SYS_Roles
                .FirstOrDefaultAsync(x => x.FormID == roleUpdateDTO.FormID)
                ?? throw new ValidationException("Role not found.");

            // Check if role already exists
            if (await DB.SYS_Roles.AnyAsync(x => x.RoleName == roleUpdateDTO.RoleName && x.FormID != roleUpdateDTO.FormID))
                throw new ValidationException($"Role {roleUpdateDTO.RoleName} already exists.");

            // Update role
            role.RoleName = roleUpdateDTO.RoleName ?? throw new ValidationException("Role Name is required.");

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(Guid roleID)
        {
            // Get Role
            var role = await DB.SYS_Roles
                .FirstOrDefaultAsync(x => x.FormID == roleID)
                ?? throw new ValidationException("Role not found.");

            // Check if Users Assigned 
            if (await DB.SYS_Users.AnyAsync(x => x.RoleID == role.Id))
                throw new ValidationException("Role is assigned to users.");

            // Delete
            role.IsDelete = true;

            await DB.SaveChangesAsync();

        }

        public async Task<RoleDTO> GetRoleAsync(Guid roleID)
        {
            return await DB.SYS_Roles
                .Where(x => x.FormID == roleID)
                .Select(x => new RoleDTO
                {
                    FormID = x.FormID,
                    RoleName = x.RoleName
                })
                .FirstOrDefaultAsync()
                ?? throw new ValidationException("Role not found.");
        }

        public async Task<PagedTable<RoleSearchReturnDTO>> SearchRolesAsync(RoleSearchDTO searchQuery)
        {
            var query = DB.SYS_Roles
                .Include(x => x.SYS_User)
                .AsQueryable();

            // Filters
            if (!string.IsNullOrEmpty(searchQuery.SearchFilter))
            {
                var searchFilter = searchQuery.SearchFilter
                    .Split(',')
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

                foreach (var filter in searchFilter)
                    query = query.Where(x => x.RoleName.Contains(filter));
            }

            if (searchQuery.EmptyOnly)
                query = query.Where(x => !x.SYS_User.Any());

            // Apply Order By
            switch (searchQuery.SortColumn)
            {
                case 1:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.RoleName)
                        : query.OrderByDescending(x => x.RoleName);
                    break;
                case 2:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.SYS_User.Count)
                        : query.OrderByDescending(x => x.SYS_User.Count);
                    break;
                default:
                    query = query.OrderBy(x => x.Id);
                    break;
            }

            // Apply pagination and projection
            var roleList = await query
                .Skip(searchQuery.RecordSize * (searchQuery.Page - 1))
                .Take(searchQuery.RecordSize)
                .Select(x => new RoleSearchReturnDTO
                {
                    FormID = x.FormID,
                    RoleName = x.RoleName,
                    UserCount = x.SYS_User.Count
                })
                .ToListAsync();

            // Return Object
            return new PagedTable<RoleSearchReturnDTO>
            {
                Data = roleList,
                TotalRecords = await query.CountAsync(),
                PageIndex = searchQuery.Page
            };
        }

        public async Task<List<RoleToFunctionDTO>> GetRoleToFunctionByRoleIDAsync(Guid roleID)
        {
            return await DB.SYS_RolesToFunction
                .Include(x => x.SYS_Function)
                .Where(x => x.SYS_Roles.FormID == roleID)
                .Select(x => new RoleToFunctionDTO
                {
                    FormID = x.FormID,
                    FunctionName = x.SYS_Function.FunctionName,
                    CanAccess = x.CanAccess
                })
                .ToListAsync();
        }
    }
}
