using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Receiving;

namespace OrderTrak.API.Services.Receiving
{
    public interface IReceivingService
    {
        Task<Guid> CreateReceivingAsync(ReceivingCreateDTO receivingCreateDTO);
        Task DeleteReceivingAsync(Guid recID);
        //Task<PartDTO> GetPartAsync(Guid partID);
        Task<PagedTable<ReceivingSearchReturnDTO>> SearchReceivingAsync(ReceivingSearchDTO searchQuery);
    }
}
