using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.OrderTrakDB;

namespace OrderTrak.API.Services.Filters
{
    public class DropDownFilterFactory(OrderTrakContext orderTrakContext) : IDropDownFilterFactory
    {
        private readonly OrderTrakContext DB = orderTrakContext;

        public async Task<List<DropDownFilterDTO>> GetUnassignedUsers()
        {
            return await DB.SYS_Users
                .Where(x => !x.RoleID.HasValue && x.Approved)
                .Select(x => new DropDownFilterDTO
                {
                    FormID = x.FormID,
                    Label = $"{x.FirstName} {x.LastName}"
                })
                .ToListAsync();
        }
    }
}
