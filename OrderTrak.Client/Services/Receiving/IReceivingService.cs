using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Receiving
{
    public interface IReceivingService
    {
        Task<Guid> CreateReceivingAsync(ReceivingCreateDTO receivingCreateDTO);
        Task DeleteReceivingAsync(Guid recID);
        Task<ReceivingDTO> GetReceivingAsync(Guid recID);
        Task<PagedTableOfReceivingSearchReturnDTO> SearchReceivingAsync(ReceivingSearchDTO searchQuery);
        Task CreateReceivingLineAsync(ReceivingLineCreateDTO receivingLineCreateDTO);
        Task UpdateReceivingAsync(ReceivingUpdateDTO receivingUpdateDTO);
        Task<PagedTableOfReceivingPutawaySearchReturnDTO> SearchReceivingPutawayAsync(SearchQueryDTO searchQuery);
    }
}
