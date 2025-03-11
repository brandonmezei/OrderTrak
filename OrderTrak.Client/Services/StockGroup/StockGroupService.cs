using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.StockGroup
{
    public class StockGroupService(IClient client) : IStockGroupService
    {

        private readonly IClient ApiClient = client;

        public async Task<Guid> CreateStockGroupAsync(StockGroupCreateDTO stockGroupCreateDTO)
        {
            return await ApiClient.CreateStockGroupAsync(stockGroupCreateDTO);
        }

        public async Task DeleteStockGroupAsync(Guid stockGroupID)
        {
            await ApiClient.DeleteStockGroupAsync(stockGroupID);
        }

        public async Task<StockGroupDTO> GetStockGroupAsync(Guid stockGroupID)
        {
            return await ApiClient.GetStockGroupAsync(stockGroupID);
        }

        public async Task<PagedTableOfStockGroupSearchReturnDTO> SearchStockGroupAsync(SearchQueryDTO searchQuery)
        {
            return await ApiClient.SearchStockGroupAsync(searchQuery);
        }

        public async Task UpdateStockGroupAsync(StockGroupUpdateDTO stockGroupUpdateDTO)
        {
            await ApiClient.UpdateStockGroupAsync(stockGroupUpdateDTO);
        }
    }
}
