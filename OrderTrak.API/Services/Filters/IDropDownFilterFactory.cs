using OrderTrak.API.Models.DTO;

namespace OrderTrak.API.Services.Filters
{
    public interface IDropDownFilterFactory
    {
        Task<List<DropDownFilterDTO>> GetUnassignedUsersAsync();
        Task<List<DropDownFilterDTO>> GetUOMAsync();
        Task<List<DropDownFilterDTO>> GetCustomersAsync();
        Task<List<DropDownFilterDTO>> GetProjectsAsync(Guid CustomerID);
    }
}
