using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Inventory;
using OrderTrak.Client.Services.Receiving;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Receiving
{
    public partial class ReceivingPutaway
    {
        [Inject]
        private IReceivingService ReceivingService { get; set; } = default!;

        [Inject]
        private IInventoryService InventoryService { get; set; } = default!;

        protected SearchQueryDTO SearchFilters { get; set; } = new() { Page = 1, RecordSize = 50, SortOrder = 1, SortColumn = 1 };

        protected PagedTableOfReceivingPutawaySearchReturnDTO? ReturnTable;

        protected InventoryLocationUpdateDTO? PutawayInventory { get; set; }
        
        protected override void OnInitialized()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Receiving Putaway", "Update Stock to Putaway Location.");

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

                    // Get Rec from API
                    SearchFilters = await LocalStorage.GetItemAsync<SearchQueryDTO>("search") ?? SearchFilters;
                    ReturnTable = await ReceivingService.SearchReceivingPutawayAsync(SearchFilters);
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

                // Get Rec from API
                ReturnTable = await ReceivingService.SearchReceivingPutawayAsync(SearchFilters);

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

                // Get Rec from API
                ReturnTable = await ReceivingService.SearchReceivingPutawayAsync(SearchFilters);

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

                // Get Rec from API
                ReturnTable = await ReceivingService.SearchReceivingPutawayAsync(SearchFilters);

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

        protected void Putaway_Toggle(Guid? LineID)
        {
            if (LineID.HasValue)
            {
                PutawayInventory = new()
                {
                    FormID = LineID.Value,
                    LocationNumber = string.Empty
                };
            }
            else
            {
                PutawayInventory = null;
            }
        }

        protected async Task Putaway_Submit()
        {
            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            try
            {
                if (PutawayInventory != null)
                {
                    // Putaway Inventory
                    await InventoryService.UpdateInventoryLocationPutawayAsync(PutawayInventory);

                    // Get Rec from API
                    ReturnTable = await ReceivingService.SearchReceivingPutawayAsync(SearchFilters);
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
                PutawayInventory = null;
                IsLoading = false;
            }
        }
    }
}