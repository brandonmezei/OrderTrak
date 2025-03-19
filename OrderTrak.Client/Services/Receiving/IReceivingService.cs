using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Receiving
{
    public interface IReceivingService
    {
        Task<Guid> CreateReceivingAsync(ReceivingCreateDTO receivingCreateDTO);
        Task DeleteReceivingAsync(Guid recID);
        //Task<PartDTO> GetPartAsync(Guid partID);
        Task<PagedTableOfReceivingSearchReturnDTO> SearchReceivingAsync(ReceivingSearchDTO searchQuery);
    }
}
