using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Order;
using static OrderTrak.Client.Models.OrderTrakMessages;
using System.Threading.Channels;
using OrderTrak.Client.Statics;

namespace OrderTrak.Client.Pages.Order
{
    public partial class OrderSection1
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private IOrderService OrderService { get; set; } = default!;

        protected OrderHeaderDTO? Order { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();

            try
            {
                Order = await OrderService.GetOrderHeaderAsync(FormID);

                Layout.UpdateHeader("Order Admin", $"Order: {Order.OrderID}");
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

        protected async Task Save_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (Order != null)
                {

                    // Save Upper Level Info
                    await OrderService.UpdateOrderHeaderAsync(MapperService.Map<OrderHeaderUpdateDTO>(Order));

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