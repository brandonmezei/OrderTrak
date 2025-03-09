using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.StockGroup;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.StockGroup
{
    public partial class StockGroupSearch
    {
        [Inject]
        private IStockGroupService StockGroupService { get; set; } = default!;

        [SupplyParameterFromQuery]
        public bool Delete { get; set; }

        protected SearchQueryDTO SearchFilters { get; set; } = new() { Page = 1, RecordSize = 50, SortOrder = 1, SortColumn = 1 };

        protected StockGroupCreateDTO? CreateStockGroup { get; set; }

        protected PagedTableOfStockGroupSearchReturnDTO? ReturnTable;

        protected Guid? DeleteID { get; set; }

        protected override void OnInitialized()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Stock Group Admin", "Create and edit stock groups.");

            if (Delete)
                Layout.AddMessage(Messages.DeleteSuccessful, MessageType.Success);

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

                    // Get StockGroups from API
                    SearchFilters = await LocalStorage.GetItemAsync<SearchQueryDTO>("search") ?? SearchFilters;
                    ReturnTable = await StockGroupService.SearchStockGroupAsync(SearchFilters);
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

                // Get Stock Group from API
                ReturnTable = await StockGroupService.SearchStockGroupAsync(SearchFilters);

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

                // Get Stock Group from API
                ReturnTable = await StockGroupService.SearchStockGroupAsync(SearchFilters);

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

                // Get Stock Group from API
                ReturnTable = await StockGroupService.SearchStockGroupAsync(SearchFilters);

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

        protected void CreateStockGroup_Toggle()
        {
            if (CreateStockGroup == null)
                CreateStockGroup = new();
            else
                CreateStockGroup = null;
        }

        protected async Task CreateStockGroup_Submit()
        {
            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            try
            {
                if (CreateStockGroup != null)
                {
                    // Create the StockGroup
                    await StockGroupService.CreateStockGroupAsync(CreateStockGroup);

                    // Reload Location List
                    ReturnTable = await StockGroupService.SearchStockGroupAsync(SearchFilters);
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
                CreateStockGroup = null;
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
                    // Delete the Stock Group
                    await StockGroupService.DeleteStockGroupAsync(DeleteID.Value);

                    // Reload Location List
                    ReturnTable = await StockGroupService.SearchStockGroupAsync(SearchFilters);
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
    }
}