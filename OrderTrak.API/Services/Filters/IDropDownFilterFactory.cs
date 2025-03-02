using OrderTrak.API.Models.DTO;

namespace OrderTrak.API.Services.Filters
{
    public interface IDropDownFilterFactory
    {
        public Task<List<DropDownFilterDTO>> GetUnassignedUsers();
    }
}
