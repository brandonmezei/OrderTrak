using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Receiving
{
    public class ReceivingService(IClient client) : IReceivingService
    {
        private readonly IClient ApiService = client;
        public async Task<Guid> CreateReceivingAsync(ReceivingCreateDTO receivingCreateDTO)
        {
            return await ApiService.CreateReceivingAsync(receivingCreateDTO);
        }

        public async Task DeleteReceivingAsync(Guid recID)
        {
            await ApiService.DeleteReceivingAsync(recID);
        }

        public async Task<PagedTableOfReceivingSearchReturnDTO> SearchReceivingAsync(ReceivingSearchDTO searchQuery)
        {
            return await ApiService.SearchReceivingAsync(searchQuery);
        }
    }
}
