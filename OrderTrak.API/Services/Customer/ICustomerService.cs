using OrderTrak.API.Models.DTO.Customer;

namespace OrderTrak.API.Services.Customer
{
    public interface ICustomerService
    {
        Task<Guid> CreateCustomerAsync(CustomerCreateDTO customerCreateDTO);
    }

}
