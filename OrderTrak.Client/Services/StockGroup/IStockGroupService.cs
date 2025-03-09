using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.StockGroup
{
    public interface IStockGroupService
    {
        Task<Guid> CreateStockGroupAsync(StockGroupCreateDTO stockGroupCreateDTO);
        Task UpdateStockGroupAsync(StockGroupUpdateDTO stockGroupUpdateDTO);
        Task DeleteStockGroupAsync(Guid stockGroupID);
        Task<StockGroupDTO> GetStockGroupAsync(Guid stockGroupID);
        Task<PagedTableOfStockGroupSearchReturnDTO> SearchStockGroupAsync(SearchQueryDTO searchQuery);
    }
}
