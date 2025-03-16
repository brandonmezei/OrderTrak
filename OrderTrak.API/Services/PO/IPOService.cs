using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.PO;

namespace OrderTrak.API.Services.PO
{
    public interface IPOService
    {
        Task<Guid> CreatePOAsync(POCreateDTO pOCreateDTO);
        Task UpdatePOAsync(POUpdateDTO pOUpdateDTO);
        Task DeletePOAsync(Guid poID);
        Task<PoDTO> GetPOAsync(Guid poID);
        Task<PagedTable<POSearchReturnDTO>> SearchPOAsync(POSearchDTO searchQuery);
        Task CreatePOLineAsync(POCreateLineDTO poLineCreateDTO);
        Task DeletePOLineAsync(Guid FormID);
        Task UpdatePOLineAsync(POUpdateLineDTO poLineUpdateDTO);
    }
}
