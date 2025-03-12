using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.PO
{
    public class POService(IClient client) : IPOService
    {
        private readonly IClient APIService = client;

        public async Task<Guid> CreatePOAsync(POCreateDTO pOCreateDTO)
        {
            return await APIService.CreatePOAsync(pOCreateDTO);
        }

        public async Task DeletePOAsync(Guid partID)
        {
            await APIService.DeletePOAsync(partID);
        }

        public async Task<PoDTO> GetPOAsync(Guid partID)
        {
            return await APIService.GetPOAsync(partID);
        }

        public async Task<PagedTableOfPOSearchReturnDTO> SearchPOAsync(POSearchDTO searchQuery)
        {
            return await APIService.SearchPOAsync(searchQuery);
        }

        public async Task UpdatePOAsync(POUpdateDTO pOUpdateDTO)
        {
            await APIService.UpdatePOAsync(pOUpdateDTO);
        }
    }
}
