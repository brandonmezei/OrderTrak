using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Customer
{
    public class CustomerService(IClient client) : ICustomerService
    {

        private readonly IClient _apiService = client;

        public async Task<PagedTableOfCustomerSearchReturnDTO> SearchCustomersAsync(CustomerSearchDTO searchDTO)
        {
            return await _apiService.SearchCustomerAsync(searchDTO);
        }
    }
}
