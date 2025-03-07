using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Parts
{
    public class PartService(IClient client) : IPartService
    {
        private readonly IClient ApiService = client;

        public async Task<Guid> CreatePartAsync(PartCreateDTO partCreateDTO)
        {
            return await ApiService.CreatePartAsync(partCreateDTO);
        }

        public async Task DeletePartAsync(Guid partID)
        {
            await ApiService.DeletePartAsync(partID);
        }

        public async Task<PartDTO> GetPartAsync(Guid partID)
        {
            return await ApiService.GetPartAsync(partID);
        }

        public async Task<PagedTableOfPartSearchReturnDTO> SearchPartsAsync(PartSearchDTO searchQuery)
        {
            return await ApiService.SearchPartAsync(searchQuery);
        }

        public async Task UpdatePartAsync(PartUpdateDTO partUpdateDTO)
        {
           await ApiService.UpdatePartAsync(partUpdateDTO);
        }
    }
}
