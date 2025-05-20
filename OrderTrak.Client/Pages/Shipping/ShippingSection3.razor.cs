using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Order;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Shipping
{
    public partial class ShippingSection3
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private IOrderService OrderService { get; set; } = default!;

        protected OrderHeaderDTO? Order { get; set; }

        protected OrderShipDTO? OrderShipping { get; set; }

        protected SearchQueryDTO SearchFilters { get; set; } = new() { Page = 1, RecordSize = 50, SortOrder = 1, SortColumn = 1 };


        protected PagedTableOfOrderTrackingSearchReturnDTO? ReturnTable;

        protected OrderCreateTrackingDTO? CreateTracking { get; set; }

        protected Guid? DeleteID { get; set; }

        protected bool ShipConfirm { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();

            try
            {
                Order = await OrderService.GetOrderHeaderAsync(FormID);
                OrderShipping = await OrderService.GetOrderShippingAsync(FormID);

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

                    // Get Tracking from API
                    SearchFilters = await LocalStorage.GetItemAsync<SearchQueryDTO>("search") ?? SearchFilters;
                    ReturnTable = await OrderService.SearchOrderTrackingAsync(SearchFilters);
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

        protected async Task Search_Click()
        {
            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            try
            {
                SearchFilters.Page = 1;

                // Save Filters
                await LocalStorage.SetItemAsync("search", SearchFilters);

                // Get Tracking from API
                ReturnTable = await OrderService.SearchOrderTrackingAsync(SearchFilters);

                if (ReturnTable?.TotalRecords == 0)
                {
                    Layout.AddMessage(Messages.NoRecordsFound, MessageType.Warning);
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
            finally
            {
                IsLoading = false;
            }
        }

        protected async Task SortSwitch_Click(int column)
        {
            SearchFilters.SortColumn = column;
            SearchFilters.SortOrder = SearchFilters.SortOrder == 1 ? 2 : 1;

            try
            {
                // Save Filters
                await LocalStorage.SetItemAsync("search", SearchFilters);

                // Get Tracking from API
                ReturnTable = await OrderService.SearchOrderTrackingAsync(SearchFilters);

                if (ReturnTable?.TotalRecords == 0)
                {
                    Layout.AddMessage(Messages.NoRecordsFound, MessageType.Warning);
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

        protected async Task PageSwitch_Click(int page)
        {
            SearchFilters.Page = page;

            try
            {
                // Save Filters
                await LocalStorage.SetItemAsync("search", SearchFilters);

                // Get Tracking from API
                ReturnTable = await OrderService.SearchOrderTrackingAsync(SearchFilters);

                if (ReturnTable?.TotalRecords == 0)
                {
                    Layout.AddMessage(Messages.NoRecordsFound, MessageType.Warning);
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

        protected void CreateTracking_Toggle()
        {
            Layout.Messages.Clear();

            if (CreateTracking == null)
            {
                if (Order != null)
                    CreateTracking = new() { FormID = Order.FormID };
            }
            else
                CreateTracking = null;
        }

        protected async Task CreateTracking_Submit()
        {
            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            try
            {
                if (CreateTracking != null)
                {
                    // Create the Tracking
                    await OrderService.CreateTrackingForOrderAsync(CreateTracking);

                    // Reload Tracking
                    ReturnTable = await OrderService.SearchOrderTrackingAsync(SearchFilters);

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
            finally
            {
                CreateTracking = null;
                IsLoading = false;
            }
        }

        protected void Delete_Toggle(Guid? FormID)
        {
            Layout.ClearMessages();

            DeleteID = FormID;
        }

        protected async Task DeleteConfirm_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (DeleteID.HasValue)
                {
                    // Delete the Tracking
                    await OrderService.DeleteTrackingFromOrderAsync(DeleteID.Value);

                    // Reload Tracking
                    ReturnTable = await OrderService.SearchOrderTrackingAsync(SearchFilters);

                    Layout.AddMessage(Messages.DeleteSuccessful, MessageType.Success);
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
            finally
            {
                DeleteID = null;
            }
        }

        protected void ShipConfirm_Toggle()
        {
            Layout.ClearMessages();
            ShipConfirm = !ShipConfirm;
        }

        protected async Task ShipConfirm_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (Order != null)
                {
                    // Ship Order
                    await OrderService.CompleteShippingOrderAsync(new OrderCompleteShippingDTO { OrderID = Order.FormID });

                    // Redirect
                    Navigation.NavigateTo($"/shipping/search?ShippedID={Order.OrderID}");
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