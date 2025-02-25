using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Customer
{
    public interface ICustomerService
    {
        Task<PagedTableOfCustomerSearchReturnDTO> SearchCustomersAsync(CustomerSearchDTO searchDTO);
        Task<Guid> CreateCustomerAsync(CustomerCreateDTO customerCreateDTO);
        Task UpdateCustomerAsync(CustomerUpdateDTO customerUpdateDTO);
        Task DeleteCustomerAsync(Guid customerId);
        Task<CustomerDTO> GetCustomerAsync(Guid customerId);
    }
}
