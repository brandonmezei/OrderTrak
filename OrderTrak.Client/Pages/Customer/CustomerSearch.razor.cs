using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Customer;

namespace OrderTrak.Client.Pages.Customer
{
    public partial class CustomerSearch
    {
        [Inject]
        private ICustomerService _customerService { get; set; } = default!;
        protected CustomerSearchDTO searchFilters { get; set; } = new() { Page = 1, RecordSize = 50 };
        protected PagedTableOfCustomerSearchReturnDTO ReturnTable { get; set; } = new();

        protected override void OnInitialized()
        {
            Layout.UpdateHeader("Customer Admin", "Create and edit customers. Add projects to customers.");
        }
    }
}