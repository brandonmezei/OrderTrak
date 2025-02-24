using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Customer
{
    public interface ICustomerService
    {
        Task<PagedTableOfCustomerSearchReturnDTO> SearchCustomersAsync(CustomerSearchDTO searchDTO);
    }
}
