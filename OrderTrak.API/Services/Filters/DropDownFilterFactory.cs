using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.OrderTrakDB;

namespace OrderTrak.API.Services.Filters
{
    public class DropDownFilterFactory(OrderTrakContext orderTrakContext) : IDropDownFilterFactory
    {
        private readonly OrderTrakContext DB = orderTrakContext;

        public async Task<List<DropDownFilterDTO>> GetUnassignedUsersAsync()
        {
            return await DB.SYS_Users
                .Where(x => !x.RoleID.HasValue && x.Approved)
                .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName)
                .AsNoTracking()
                .Select(x => new DropDownFilterDTO
                {
                    FormID = x.FormID,
                    Label = $"{x.FirstName} {x.LastName}"
                })
                .ToListAsync();
        }

        public async Task<List<DropDownFilterDTO>> GetUOMAsync()
        {
            return await DB.UPL_UOM
                .OrderBy(x => x.UnitOfMeasurement)
                .AsNoTracking()
                .Select(x => new DropDownFilterDTO
                {
                    FormID = x.FormID,
                    Label = x.UnitOfMeasurement
                })
                .ToListAsync();
        }
    }
}
