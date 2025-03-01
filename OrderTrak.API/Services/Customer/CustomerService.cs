using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Customer;
using OrderTrak.API.Models.OrderTrakDB;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Services.Customer
{
    public class CustomerService(OrderTrakContext orderTrakContext) : ICustomerService
    {
        private readonly OrderTrakContext DB = orderTrakContext;

        public async Task<Guid> CreateCustomerAsync(CustomerCreateDTO customerCreateDTO)
        {
            // Check if customer already exists
            if (await DB.UPL_Customer.AnyAsync(x => x.CustomerCode == customerCreateDTO.CustomerCode))
                throw new ValidationException("Customer already exists");

            // Create new customer
            var newCustomer = new UPL_Customer
            {
                CustomerCode = customerCreateDTO.CustomerCode ?? throw new ValidationException("Customer Code is required"),
                CustomerName = customerCreateDTO.CustomerName ?? throw new ValidationException("Customer Name is required"),
                Address = customerCreateDTO.Address ?? throw new ValidationException("Address is required"),
                Address2 = customerCreateDTO.Address2,
                City = customerCreateDTO.City ?? throw new ValidationException("City is required"),
                State = customerCreateDTO.State ?? throw new ValidationException("State is required"),
                Zip = customerCreateDTO.Zip ?? throw new ValidationException("Zip is required"),
                Phone = customerCreateDTO.Phone ?? throw new ValidationException("Customer Code is required")
            };

            // Save
            await DB.UPL_Customer.AddAsync(newCustomer);
            await DB.SaveChangesAsync();

            return newCustomer.FormID;
        }

        public async Task UpdateCustomerAsync(CustomerUpdateDTO customerUpdateDTO)
        {
            // Get Customer
            var customer = await DB.UPL_Customer
                .FirstOrDefaultAsync(x => x.FormID == customerUpdateDTO.FormID)
                ?? throw new ValidationException("Customer not found");

            // Check if customer already exists
            if (await DB.UPL_Customer.AnyAsync(x => x.CustomerCode == customerUpdateDTO.CustomerCode && x.FormID != customerUpdateDTO.FormID))
                throw new ValidationException("Customer already exists");

            // Update customer
            customer.CustomerCode = customerUpdateDTO.CustomerCode ?? throw new ValidationException("Customer Code is required");
            customer.CustomerName = customerUpdateDTO.CustomerName ?? throw new ValidationException("Customer Name is required");
            customer.Address = customerUpdateDTO.Address ?? throw new ValidationException("Address is required");
            customer.Address2 = customerUpdateDTO.Address2;
            customer.City = customerUpdateDTO.City ?? throw new ValidationException("City is required");
            customer.State = customerUpdateDTO.State ?? throw new ValidationException("State is required");
            customer.Zip = customerUpdateDTO.Zip ?? throw new ValidationException("Zip is required");
            customer.Phone = customerUpdateDTO.Phone ?? throw new ValidationException("Phone is required");

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(Guid customerId)
        {
            // Get Customer
            var customer = await DB.UPL_Customer
                .FirstOrDefaultAsync(x => x.FormID == customerId)
                ?? throw new ValidationException("Customer not found.");

            // Delete
            customer.IsDelete = true;

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task<CustomerDTO> GetCustomerAsync(Guid customerId)
        {
            // Get Customer
            return await DB.UPL_Customer
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
                    Phone = x.Phone
                })
                .FirstOrDefaultAsync()
                ?? throw new ValidationException("Customer not found.");
        }

        public async Task<PagedTable<CustomerSearchReturnDTO>> SearchCustomersAsync(CustomerSearchDTO searchQuery)
        {
            // Get Change Logs
            var query = DB.UPL_Customer
                .Include(x => x.UPL_Projects)
                .AsQueryable();

            // Filters
            if (!string.IsNullOrEmpty(searchQuery.SearchFilter))
            {
                var searchFilter = searchQuery.SearchFilter
                    .Split(',')
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

                foreach (var filter in searchFilter)
                {
                    query = query.Where(x => x.CustomerCode.Contains(filter) ||
                                             x.CustomerName.Contains(filter) ||
                                             x.Address.Contains(filter) ||
                                             x.City.Contains(filter) ||
                                             x.State.Contains(filter) ||
                                             x.Zip.Contains(filter) ||
                                             x.Phone.Contains(filter));
                }
            }

            if (searchQuery.EmptyOnly)
                query = query.Where(x => !x.UPL_Projects.Any());

            // Apply Order By
            switch (searchQuery.SortColumn)
            {
                case 1:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.CustomerCode)
                        : query.OrderByDescending(x => x.CustomerCode);
                    break;
                case 2:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.CustomerName)
                        : query.OrderByDescending(x => x.CustomerName);
                    break;
                case 3:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.Phone)
                        : query.OrderByDescending(x => x.Phone);
                    break;
                case 4:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.UPL_Projects.Count)
                        : query.OrderByDescending(x => x.UPL_Projects.Count);
                    break;
                default:
                    query = query.OrderBy(x => x.Id);
                    break;
            }

            // Apply pagination and projection
            var customerList = await query
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
