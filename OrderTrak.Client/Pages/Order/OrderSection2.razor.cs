using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Order;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Order
{
    public partial class OrderSection2
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private IOrderService OrderService { get; set; } = default!;

        protected List<OrderPartListDTO>? PartList { get; set; }
        protected List<OrderPartListDTO>? FilteredPartList { get; set; }

        protected override void OnInitialized()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Order Admin", "Create and edit orders.");

            IsCardLoading = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    // Sleep for 500ms to allow the page to render before loading the data
                    await Task.Delay(500);

                    PartList = await OrderService.GetOrderLineAsync(FormID);
                    FilteredPartList = PartList;
                }
                catch (ApiException ex)
                {
                    Layout.AddMessage(ex.Response, MessageType.Error);
                }
                catch (Exception ex)
                {
                    Layout.AddMessage(ex.Message, MessageType.Error);
                }
                finally
                {
                    IsCardLoading = false;
                    StateHasChanged();
                }
            }
        }
    }
}