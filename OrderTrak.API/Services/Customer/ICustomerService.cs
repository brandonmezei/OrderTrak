using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Customer;

namespace OrderTrak.API.Services.Customer
{
    public interface ICustomerService
    {
        Task<Guid> CreateCustomerAsync(CustomerCreateDTO customerCreateDTO);
        Task UpdateCustomerAsync(CustomerUpdateDTO customerUpdateDTO);
        Task DeleteCustomerAsync(Guid customerId);
        Task<CustomerDTO> GetCustomerAsync(Guid customerId);
        Task<PagedTable<CustomerSearchReturnDTO>> SearchCustomersAsync(CustomerSearchDTO customerSearchDTO);
    }

}
