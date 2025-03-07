using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Parts
{
    public interface IPartService
    {
        Task<Guid> CreatePartAsync(PartCreateDTO partCreateDTO);
        Task UpdatePartAsync(PartUpdateDTO partUpdateDTO);
        Task DeletePartAsync(Guid partID);
        Task<PartDTO> GetPartAsync(Guid partID);
        Task<PagedTableOfPartSearchReturnDTO> SearchPartsAsync(PartSearchDTO searchQuery);
    }
}
