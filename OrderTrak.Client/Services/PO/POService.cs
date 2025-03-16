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

        public async Task CreatePOLine(POCreateLineDTO poLineCreateDTO)
        {
            await APIService.CreatePOLineAsync(poLineCreateDTO);
        }

        public async Task DeletePOAsync(Guid partID)
        {
            await APIService.DeletePOAsync(partID);
        }

        public async Task DeletePOLineAsync(Guid FormID)
        {
            await APIService.DeletePOLineAsync(FormID);
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

        public async Task UpdatePOLineAsync(POUpdateLineDTO poLineUpdateDTO)
        {
            await APIService.UpdatePOLineAsync(poLineUpdateDTO);
        }
    }
}
