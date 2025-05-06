using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Order;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Order
{
    public partial class OrderSection3
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private IOrderService OrderService { get; set; } = default!;

        protected OrderHeaderDTO? Order { get; set; }

        protected OrderShipDTO? OrderShipping { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Order Admin", "Create and edit orders.");

            try
            {
                Order = await OrderService.GetOrderHeaderAsync(FormID);
                OrderShipping = await OrderService.GetOrderShippingAsync(FormID);
            }
            catch (ApiException ex)
            {
                Layout.AddMessage(ex.Response, MessageType.Error);
            }
            catch (Exception ex)
            {
                Layout.AddMessage(ex.Message, MessageType.Error);
            }

            IsCardLoading = true;
        }

        protected async Task Save_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (OrderShipping != null)
                {

                    // Save Upper Level Info
                    await OrderService.UpdateOrderShippingAsync(MapperService.Map<OrderShipUpdateDTO>(OrderShipping));

                    Layout.AddMessage(Messages.SaveSuccesful, MessageType.Success);
                }
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