using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Order;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Order
{
    public partial class OrderSearch
    {
        [Inject]
        private IOrderService OrderService { get; set; } = default!;

        [SupplyParameterFromQuery]
        public bool Cancel { get; set; }

        protected SearchQueryDTO SearchFilters { get; set; } = new() { Page = 1, RecordSize = 50, SortOrder = 1, SortColumn = 1 };

        protected OrderCreateDTO? CreateOrder { get; set; }

        protected PagedTableOfOrderSearchReturnDTO? ReturnTable;

        protected override void OnInitialized()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Order Admin", "Create and edit orders.");

            if (Cancel)
                Layout.AddMessage(Messages.CancelSuccessful, MessageType.Success);

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

                    // Get Orders from API
                    SearchFilters = await LocalStorage.GetItemAsync<SearchQueryDTO>("search") ?? SearchFilters;
                    ReturnTable = await OrderService.SearchOrderAsync(SearchFilters);
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

                // Get Order from API
                ReturnTable = await OrderService.SearchOrderAsync(SearchFilters);

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

                // Get Order from API
                ReturnTable = await OrderService.SearchOrderAsync(SearchFilters);

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

                // Get Order from API
                ReturnTable = await OrderService.SearchOrderAsync(SearchFilters);

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

        protected void CreateOrder_Toggle()
        {
            if (CreateOrder == null)
                CreateOrder = new();
            else
                CreateOrder = null;
        }

        protected async Task CreateOrder_Submit()
        {
            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            try
            {
                if (CreateOrder != null)
                {
                    // Create the Order
                    await OrderService.CreateOrderAsync(CreateOrder);

                    // Get Order from API
                    ReturnTable = await OrderService.SearchOrderAsync(SearchFilters);

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
                CreateOrder = null;
                IsLoading = false;
            }
        }

        protected void Project_Change(Guid? FormID)
        {
            if (CreateOrder != null && FormID.HasValue)
                CreateOrder.ProjectID = FormID.Value;
        }
    }
}