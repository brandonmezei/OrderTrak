using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO.Customer;
using OrderTrak.API.Models.OrderTrakDB;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly OrderTrakContext _orderTrakContext;

        public CustomerService(OrderTrakContext orderTrakContext)
        {
            _orderTrakContext = orderTrakContext;
        }

        public async Task<Guid> CreateCustomerAsync(CustomerCreateDTO customerCreateDTO)
        {
            // Check if customer already exists
            if (await _orderTrakContext.UPL_Customer.AnyAsync(x => x.CustomerCode == customerCreateDTO.CustomerCode))
                throw new ValidationException("Customer already exists");

            // Create new customer
            var newCustomer = new UPL_Customer
            {
                CustomerCode = customerCreateDTO.CustomerCode,
                CustomerName = customerCreateDTO.CustomerName,
                Address = customerCreateDTO.Address,
                Address2 = customerCreateDTO.Address2,
                City = customerCreateDTO.City,
                State = customerCreateDTO.State,
                Zip = customerCreateDTO.Zip,
                Phone = customerCreateDTO.Phone
            };

            // Save
            await _orderTrakContext.UPL_Customer.AddAsync(newCustomer);
            await _orderTrakContext.SaveChangesAsync();

            return newCustomer.FormID;
        }
    }
}
