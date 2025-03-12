using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.PO
{
    public interface IPOService
    {
        Task<Guid> CreatePOAsync(POCreateDTO pOCreateDTO);
        Task UpdatePOAsync(POUpdateDTO pOUpdateDTO);
        Task DeletePOAsync(Guid poID);
        Task<PoDTO> GetPOAsync(Guid poID);
        Task<PagedTableOfPOSearchReturnDTO> SearchPOAsync(POSearchDTO searchQuery);
    }
}
