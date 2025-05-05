using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Filters;
using OrderTrak.API.Models.OrderTrakDB;
using OrderTrak.API.Static;

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

        public async Task<List<DropDownFilterDTO>> GetStockGroupAsync()
        {
            return await DB.UPL_StockGroup
                 .OrderBy(x => x.StockGroupTitle)
                 .AsNoTracking()
                 .Select(x => new DropDownFilterDTO
                 {
                     FormID = x.FormID,
                     Label = x.StockGroupTitle
                 })
                 .ToListAsync();
        }

        public async Task<List<DropDownFilterDTO>> GetPOListGroupAsync(POListFilterDTO pOListFilterDTO)
        {
            return await DB.PO_Header
                .Where(x => x.UPL_Project.FormID == pOListFilterDTO.ProjectID
                    && x.PO_Line.Any(i => i.UPL_PartInfo.FormID == pOListFilterDTO.PartID
                    && i.INV_Stock.Any(s => s.INV_StockStatus.StockStatus == StockStatus.InStock))
                )
                .OrderBy(x => x.PONumber)
                .AsNoTracking()
                .Select(x => new DropDownFilterDTO
                {
                    FormID = x.FormID,
                    Label = x.PONumber
                })
                .ToListAsync();
        }
    }
}
