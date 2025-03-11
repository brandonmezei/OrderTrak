using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.PO;

namespace OrderTrak.API.Services.PO
{
    public interface IPOService
    {
        Task<Guid> CreatePOAsync(POCreateDTO pOCreateDTO);
        Task UpdatePOAsync(POUpdateDTO pOUpdateDTO);
        Task DeletePOAsync(Guid partID);
        Task<PoDTO> GetPOAsync(Guid partID);
        Task<PagedTable<POSearchReturnDTO>> SearchPOAsync(POSearchDTO searchQuery);
    }
}
