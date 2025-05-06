using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Filters
{
    public interface IDropDownFactoryService
    {
        Task<List<DropDownFilterDTO>> GetUnassignedUsers();
        Task<List<DropDownFilterDTO>> GetUOMAsync();
        Task<List<DropDownFilterDTO>> GetCustomersAsync();
        Task<List<DropDownFilterDTO>> GetProjectsAsync(Guid CustomerID);
        Task<List<DropDownFilterDTO>> GetStockGroupAsync();
        Task<List<DropDownFilterDTO>> GetPOListGroupAsync(POListFilterDTO pOListFilterDTO);
        Task<List<DropDownFilterDTO>> GetOrderStatusListAsync();
    }
}
