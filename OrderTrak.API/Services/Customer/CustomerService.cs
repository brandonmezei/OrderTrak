using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Customer;
using OrderTrak.API.Models.OrderTrakDB;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Services.Customer
{
    public class CustomerService(OrderTrakContext orderTrakContext) : ICustomerService
    {
        private readonly OrderTrakContext _orderTrakContext = orderTrakContext;

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

        public async Task UpdateCustomerAsync(CustomerUpdateDTO customerUpdateDTO)
        {
            // Get Customer
            var customer = await _orderTrakContext.UPL_Customer
                .FirstOrDefaultAsync(x => x.FormID == customerUpdateDTO.FormID)
                ?? throw new ValidationException("Customer not found");

            // Check if customer already exists
            if (await _orderTrakContext.UPL_Customer.AnyAsync(x => x.CustomerCode == customerUpdateDTO.CustomerCode && x.FormID != customerUpdateDTO.FormID))
                throw new ValidationException("Customer already exists");

            // Update customer
            customer.CustomerCode = customerUpdateDTO.CustomerCode;
            customer.CustomerName = customerUpdateDTO.CustomerName;
            customer.Address = customerUpdateDTO.Address;
            customer.Address2 = customerUpdateDTO.Address2;
            customer.City = customerUpdateDTO.City;
            customer.State = customerUpdateDTO.State;
            customer.Zip = customerUpdateDTO.Zip;
            customer.Phone = customerUpdateDTO.Phone;

            // Save
            await _orderTrakContext.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(Guid customerId)
        {
            // Get Customer
            var customer = await _orderTrakContext.UPL_Customer
                .FirstOrDefaultAsync(x => x.FormID == customerId)
                ?? throw new ValidationException("Customer not found");

            // Delete
            customer.IsDelete = true;

            // Save
            await _orderTrakContext.SaveChangesAsync();
        }

        public async Task<CustomerDTO> GetCustomerAsync(Guid customerId)
        {
            // Get Customer
            return await _orderTrakContext.UPL_Customer
                .Include(x => x.UPL_Projects)
                .Where(x => x.FormID == customerId)
                .Select(x => new CustomerDTO
                {
                    FormID = x.FormID,
                    CustomerCode = x.CustomerCode,
                    CustomerName = x.CustomerName,
                    Address = x.Address,
                    Address2 = x.Address2,
                    City = x.City,
                    State = x.State,
                    Zip = x.Zip,
                    Phone = x.Phone,
                    ProjectList = x.UPL_Projects.Select(i => new CustomerProjectListDTO
                    {
                        FormID = i.FormID,
                        ProjectCode = i.ProjectCode,
                        ProjectName = i.ProjectName
                    }).ToList()
                })
                .FirstOrDefaultAsync()
                ?? throw new ValidationException("Customer not found");
        }

        public async Task<PagedTable<CustomerSearchReturnDTO>> SearchCustomersAsync(CustomerSearchDTO searchQuery)
        {
            // Get Change Logs
            var query = _orderTrakContext.UPL_Customer
                .Include(x => x.UPL_Projects)
                .AsQueryable();

            // Filters
            if (!string.IsNullOrEmpty(searchQuery.CustomerCode))
                query = query.Where(x => x.CustomerCode.Contains(searchQuery.CustomerCode));

            if (!string.IsNullOrEmpty(searchQuery.Address))
                query = query.Where(x => x.Address.Contains(searchQuery.Address));

            if (!string.IsNullOrEmpty(searchQuery.City))
                query = query.Where(x => x.City.Contains(searchQuery.City));

            if (!string.IsNullOrEmpty(searchQuery.State))
                query = query.Where(x => x.State.Contains(searchQuery.State));

            if (!string.IsNullOrEmpty(searchQuery.Zip))
                query = query.Where(x => x.Zip.Contains(searchQuery.Zip));

            if (!string.IsNullOrEmpty(searchQuery.Phone))
                query = query.Where(x => x.Phone.Contains(searchQuery.Phone));

            // Apply pagination and projection
            var customerList = await query
                .OrderBy(x => x.Id)
                .Skip(searchQuery.RecordSize * (searchQuery.Page - 1))
                .Take(searchQuery.RecordSize)
                .Select(x => new CustomerSearchReturnDTO
                {
                    FormID = x.FormID,
                    CustomerCode = x.CustomerCode,
                    CustomerName = x.CustomerName,
                    Phone = x.Phone,
                    ProjectCount = x.UPL_Projects.Count
                })
                .ToListAsync();

            // Return Object
            return new PagedTable<CustomerSearchReturnDTO>
            {
                Data = customerList,
                TotalRecords = await query.CountAsync(),
                PageIndex = searchQuery.Page
            };
        }
    }
}
