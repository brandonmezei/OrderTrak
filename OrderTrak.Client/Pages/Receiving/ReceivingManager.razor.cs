using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Receiving;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Receiving
{
    public partial class ReceivingManager
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private IReceivingService ReceivingService { get; set; } = default!;

        protected ReceivingDTO? Receiving { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Receiving", "Receive inventory to stock.");

            try
            {
                Receiving = await ReceivingService.GetReceivingAsync(FormID);

                // Get Rec Line by filter
                //FilteredPOList = [.. PurchaseOrder.PoLines];
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