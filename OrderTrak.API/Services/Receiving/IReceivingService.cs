using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Receiving;

namespace OrderTrak.API.Services.Receiving
{
    public interface IReceivingService
    {
        Task<Guid> CreateReceivingAsync(ReceivingCreateDTO receivingCreateDTO);
        Task DeleteReceivingAsync(Guid recID);
        Task<ReceivingDTO> GetReceivingAsync(Guid recID);
        Task UpdateReceivingAsync(ReceivingUpdateDTO receivingUpdateDTO);
        Task<PagedTable<ReceivingSearchReturnDTO>> SearchReceivingAsync(ReceivingSearchDTO searchQuery);
        Task CreateReceivingLineAsync(ReceivingLineCreateDTO receivingLineCreateDTO);
        Task<PagedTable<ReceivingPutawaySearchReturnDTO>> SearchReceivingPutawayAsync(SearchQueryDTO searchQuery);
    }
}
