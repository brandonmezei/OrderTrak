using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Inventory;

namespace OrderTrak.API.Services.Inventory
{
    public interface IInventoryService
    {
        Task<PagedTable<InventorySearchReturnDTO>> SearchInventoryAsync(InventorySearchDTO searchQuery);
    }
}
