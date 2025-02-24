using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Customer;

namespace OrderTrak.Client.Pages.Customer
{
    public partial class CustomerSearch
    {
        [Inject]
        private ICustomerService _customerService { get; set; } = default!;
        protected CustomerSearchDTO SearchFilters { get; set; } = new() { Page = 1, RecordSize = 50, SortOrder = 1, SortColumn = 1 };
        protected PagedTableOfCustomerSearchReturnDTO ReturnTable { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            Layout.UpdateHeader("Customer Admin", "Create and edit customers. Add projects to customers.");

            try
            { 
                SearchFilters = await LocalStorage.GetItemAsync<CustomerSearchDTO>("search") ?? SearchFilters;
            }
            catch 
            {
                await LocalStorage.RemoveItemAsync("search");
            }

            ReturnTable = await _customerService.SearchCustomersAsync(SearchFilters);
        }

        protected async Task Search_Click()
        {

        }
    }
}