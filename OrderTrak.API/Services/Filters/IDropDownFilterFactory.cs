using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Filters;

namespace OrderTrak.API.Services.Filters
{
    public interface IDropDownFilterFactory
    {
        Task<List<DropDownFilterDTO>> GetUnassignedUsersAsync();
        Task<List<DropDownFilterDTO>> GetUOMAsync();
        Task<List<DropDownFilterDTO>> GetCustomersAsync();
        Task<List<DropDownFilterDTO>> GetProjectsAsync(Guid CustomerID);
        Task<List<DropDownFilterDTO>> GetStockGroupAsync();
        Task<List<DropDownFilterDTO>> GetPOListGroupAsync(POListFilterDTO pOListFilterDTO);
        Task<List<DropDownFilterDTO>> GetOrderStatusListAsync();
        Task<List<DropDownFilterDTO>> GetInventoryStatusListAsync();
    }
}
