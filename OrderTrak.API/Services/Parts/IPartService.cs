using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Parts;

namespace OrderTrak.API.Services.Parts
{
    public interface IPartService
    {
        Task<Guid> CreatePartAsync(PartCreateDTO partCreateDTO);
        Task UpdatePartAsync(PartUpdateDTO partUpdateDTO);
        //Task DeleteCustomerAsync(Guid customerId);
        //Task<CustomerDTO> GetCustomerAsync(Guid customerId);
        //Task<PagedTable<CustomerSearchReturnDTO>> SearchCustomersAsync(CustomerSearchDTO customerSearchDTO);
    }
}
