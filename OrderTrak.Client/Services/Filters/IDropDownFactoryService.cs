using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Filters
{
    public interface IDropDownFactoryService
    {
        Task<List<DropDownFilterDTO>> GetUnassignedUsers();
    }
}
