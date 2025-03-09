using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.StockGroup;

namespace OrderTrak.API.Services.StockGroup
{
    public interface IStockGroupService
    {
        Task<Guid> CreateStockGroupAsync(StockGroupCreateDTO stockGroupCreateDTO);
        Task UpdateStockGroupAsync(StockGroupUpdateDTO stockGroupUpdateDTO);
        Task DeleteStockGroupAsync(Guid stockGroupID);
        Task<StockGroupDTO> GetStockGroupAsync(Guid stockGroupID);
        Task<PagedTable<StockGroupSearchReturnDTO>> SearchStockGroupAsync(SearchQueryDTO searchQuery);
    }
}
