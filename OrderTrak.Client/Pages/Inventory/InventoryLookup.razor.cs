using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Inventory;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Inventory
{
    public partial class InventoryLookup
    {
        [Inject]
        private IInventoryService InventoryService { get; set; } = default!;

        protected InventorySearchDTO SearchFilters { get; set; } = new() { Page = 1, RecordSize = 50, SortOrder = 1, SortColumn = 1 };

        protected PagedTableOfInventorySearchReturnDTO? ReturnTable;

        protected InventoryUpdateLookupDTO? LineUpdate { get; set; }

        protected readonly List<string> ExcludeStockStatus = [
            StockStatus.OnOrder, 
            StockStatus.Shipped
        ];

        protected override void OnInitialized()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Inventory Lookup", "Lookup and Edit Inventory.");
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

        protected async Task ShowShipped_Change()
        {
            SearchFilters.ShowShipped = !SearchFilters.ShowShipped;
            await Search_Click();
        }

        protected void UpdateLine_Toggle(Guid? LineID)
        {
            if (LineID.HasValue)
            {
                // Get Inventory Line
                var invLine = ReturnTable?.Data.FirstOrDefault(x => x.FormID == LineID.Value);

                if (invLine != null)
                    LineUpdate = MapperService.Map<InventoryUpdateLookupDTO>(invLine);
            }
            else
                LineUpdate = null;
        }

        protected async Task UpdateLine_Save()
        {
            if (LineUpdate != null)
            {
                try
                {
                    // Update Line
                    await InventoryService.UpdateInventoryLookupAsync(LineUpdate);

                    // Refresh
                    ReturnTable = await InventoryService.SearchInventoryAsync(SearchFilters);

                    Layout.AddMessage(Messages.SaveSuccesful, MessageType.Success);
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
                    LineUpdate = null;
                }
            }
        }

        protected void StockGroup_Change(Guid? FormID)
        {
            if (LineUpdate != null)
                LineUpdate.StockGroupID = FormID;
        }

        protected void InventoryStatus_Change(Guid? FormID)
        {
            if (LineUpdate != null)
                LineUpdate.StatusID = FormID;
        }
    }
}