using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Inventory;

namespace OrderTrak.Client.Shared.SearchComponents
{
    public partial class InventorySearch
    {
        [Inject]
        private IInventoryService InventoryService { get; set; } = default!;

        protected InventorySearchDTO SearchFilters { get; set; } = new() { Page = 1, RecordSize = 50, SortOrder = 1, SortColumn = 1 };

        protected PagedTableOfInventorySearchReturnDTO? ReturnTable;

        protected bool IsCardLoading { get; set; }
        protected bool IsLoading { get; set; }

        [Parameter]
        public Guid? OrderLineID { get; set; } = null;

        [Parameter]
        public EventCallback OnClose { get; set; }

        [Parameter]
        public EventCallback<Guid?> OnClick { get; set; }

        protected override void OnInitialized()
        {
            SearchFilters.OrderLineID = OrderLineID;
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

                    // Get Parts from API
                    ReturnTable = await InventoryService.SearchInventoryAsync(SearchFilters);
                }
                catch { }
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

            IsLoading = true;

            try
            {
                SearchFilters.Page = 1;

                // Get Parts from API
                ReturnTable = await InventoryService.SearchInventoryAsync(SearchFilters);
            }
            catch { }
            finally
            {
                IsLoading = false;
            }

            StateHasChanged();
        }

        protected async Task SortSwitch_Click(int column)
        {
            SearchFilters.SortColumn = column;
            SearchFilters.SortOrder = SearchFilters.SortOrder == 1 ? 2 : 1;

            try
            {
                // Get Parts from API
                ReturnTable = await InventoryService.SearchInventoryAsync(SearchFilters);
            }
            catch { }

            StateHasChanged();
        }

        protected async Task PageSwitch_Click(int page)
        {
            SearchFilters.Page = page;

            try
            {
                // Get Parts from API
                ReturnTable = await InventoryService.SearchInventoryAsync(SearchFilters);
            }
            catch { }

            StateHasChanged();
        }

        private async Task OnClose_Handler()
        {
            await OnClose.InvokeAsync();

            StateHasChanged();
        }

        private async Task OnClick_Handler(Guid? LineID)
        {
            await OnClick.InvokeAsync(LineID);

            StateHasChanged();
        }
    }
}