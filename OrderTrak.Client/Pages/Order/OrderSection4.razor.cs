using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Order;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Order
{
    public partial class OrderSection4
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private IOrderService OrderService { get; set; } = default!;

        protected OrderHeaderDTO? Order { get; set; }

        protected OrderActivationDTO? OrderActivation { get; set; }

        public List<string> ExcludeOrderStatus { get; set; } = [OrderStatus.Draft, OrderStatus.Hold, OrderStatus.Cancel, OrderStatus.Shipped];

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Order Admin", "Create and edit orders.");

            try
            {
                Order = await OrderService.GetOrderHeaderAsync(FormID);
                OrderActivation = await OrderService.GetOrderActivationAsync(FormID);

                Layout.UpdateHeader("Order Admin", $"Order: { Order.OrderID }");
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

        protected void Status_Change(Guid? FormID)
        {
            if (OrderActivation != null)
                OrderActivation.StatusID = FormID;
        }

        protected async Task Activation_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (OrderActivation != null && Order != null)
                {

                    // Save Activation
                    await OrderService.UpdateOrderActivationAsync(MapperService.Map<OrderActivationUpdateDTO>(OrderActivation));

                    // Redirect
                    Navigation.NavigateTo($"/order/search?ActiveID={ Order.OrderID }");

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