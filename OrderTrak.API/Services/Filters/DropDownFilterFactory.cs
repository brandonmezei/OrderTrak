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

        public async Task<List<DropDownFilterDTO>> GetCustomersAsync()
        {
            return await DB.UPL_Customer
                 .Include(x => x.UPL_Projects)
                 .Where(x => x.UPL_Projects.Count != 0)
                 .OrderBy(x => x.CustomerCode)
                 .AsNoTracking()
                 .Select(x => new DropDownFilterDTO
                 {
                     FormID = x.FormID,
                     Label = $"{x.CustomerCode} - {x.CustomerName}"
                 })
                 .ToListAsync();
        }

        public async Task<List<DropDownFilterDTO>> GetProjectsAsync(Guid CustomerID)
        {
            return await DB.UPL_Project
                .Where(x => x.UPL_Customer.FormID == CustomerID)
                .OrderBy(x => x.ProjectCode)
                .AsNoTracking()
                .Select(x => new DropDownFilterDTO
                {
                    FormID = x.FormID,
                    Label = $"{x.ProjectCode} - {x.ProjectName}"
                })
                .ToListAsync();
        }
    }
}
