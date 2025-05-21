using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Inventory
{
    public class InventoryService(IClient client) : IInventoryService
    {
        private readonly IClient ApiClient = client;

        public async Task<PagedTableOfInventorySearchReturnDTO> SearchInventoryAsync(InventorySearchDTO searchQuery)
        {
            return await ApiClient.SearchInventoryAsync(searchQuery);
        }

        public async Task UpdateInventoryLocationPutawayAsync(InventoryLocationUpdateDTO inventoryLocationUpdateDTO)
        {
            await ApiClient.UpdateInventoryLocationPutawayAsync(inventoryLocationUpdateDTO);
        }
    }
}
