using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Inventory
{
    public interface IInventoryService
    {
        Task<PagedTableOfInventorySearchReturnDTO> SearchInventoryAsync(InventorySearchDTO searchQuery);
    }
}
