using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Customer;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Customer
{
    public partial class CustomerEditor
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private ICustomerService CustomerService { get; set; } = default!;

        protected CustomerDTO? Customer { get;set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.UpdateHeader("Customer Admin", "Create and edit customers. Add projects to customers.");

            try
            {
                Customer = await CustomerService.GetCustomerAsync(FormID);
            }
            catch (ApiException ex)
            {
                Layout.AddMessage(ex.Response, MessageType.Error);
            }
            catch (Exception ex)
            {
                Layout.AddMessage(ex.Message, MessageType.Error);
            }
        }
    }
}