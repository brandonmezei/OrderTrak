using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Order;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Shipping
{
    public partial class ShippingSection1
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private IOrderService OrderService { get; set; } = default!;

        protected OrderHeaderDTO? Order { get; set; }
        protected OrderActivationDTO? OrderActivation { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();

            try
            {
                Order = await OrderService.GetOrderHeaderAsync(FormID);
                OrderActivation = await OrderService.GetOrderActivationAsync(FormID);

                Layout.UpdateHeader("Shipping", $"Order: {Order.OrderID}");
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