using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Inventory;
using OrderTrak.API.Models.OrderTrakDB;

namespace OrderTrak.API.Services.Inventory
{
    public interface IInventoryService
    {
        Task<PagedTable<InventorySearchReturnDTO>> SearchInventoryAsync(InventorySearchDTO searchQuery);

        Task<Guid> SplitBoxIDAsync(Guid FormID, int qty);

        Task UpdateInventoryLocationPutawayAsync(InventoryLocationUpdateDTO inventoryLocationUpdateDTO);
    }
}
