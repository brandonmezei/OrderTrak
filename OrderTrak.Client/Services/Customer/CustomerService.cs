using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Customer
{
    public class CustomerService(IClient client) : ICustomerService
    {

        private readonly IClient ApiService = client;

        public async Task<Guid> CreateCustomerAsync(CustomerCreateDTO customerCreateDTO)
        {
            return await ApiService.CreateCustomerAsync(customerCreateDTO);
        }

        public async Task DeleteCustomerAsync(Guid customerId)
        {
            await ApiService.DeleteCustomerAsync(customerId);
        }

        public async Task<CustomerDTO> GetCustomerAsync(Guid customerId)
        {
            return await ApiService.GetCustomerAsync(customerId);
        }

        public async Task<PagedTableOfCustomerSearchReturnDTO> SearchCustomersAsync(CustomerSearchDTO searchDTO)
        {
            return await ApiService.SearchCustomerAsync(searchDTO);
        }

        public async Task UpdateCustomerAsync(CustomerUpdateDTO customerUpdateDTO)
        {
            await ApiService.UpdateCustomerAsync(customerUpdateDTO);
        }
    }
}
