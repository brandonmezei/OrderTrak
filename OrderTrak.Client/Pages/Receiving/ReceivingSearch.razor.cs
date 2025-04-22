using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Receiving;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Receiving
{
    public partial class ReceivingSearch
    {
        [Inject]
        private IReceivingService ReceivingService { get; set; } = default!;

        [SupplyParameterFromQuery]
        public bool Delete { get; set; }

        protected ReceivingSearchDTO SearchFilters { get; set; } = new() { Page = 1, RecordSize = 50, SortOrder = 1, SortColumn = 1, IsToday = true };

        protected ReceivingCreateDTO? CreateRec { get; set; }

        protected PagedTableOfReceivingSearchReturnDTO? ReturnTable;

        protected Guid? DeleteID { get; set; }

        protected override void OnInitialized()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Receiving", "Receive inventory to stock.");

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

                    // Get Rec from API
                    SearchFilters = await LocalStorage.GetItemAsync<ReceivingSearchDTO>("search") ?? SearchFilters;
                    ReturnTable = await ReceivingService.SearchReceivingAsync(SearchFilters);
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
                ReturnTable = await ReceivingService.SearchReceivingAsync(SearchFilters);

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
                ReturnTable = await ReceivingService.SearchReceivingAsync(SearchFilters);

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
                ReturnTable = await ReceivingService.SearchReceivingAsync(SearchFilters);

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

        protected void CreateRec_Toggle()
        {
            if (CreateRec == null)
                CreateRec = new();
            else
                CreateRec = null;
        }

        protected async Task CreateRec_Submit()
        {
            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            try
            {
                if (CreateRec != null)
                {
                    // Create the Rec
                    await ReceivingService.CreateReceivingAsync(CreateRec);

                    // Get Rec from API
                    ReturnTable = await ReceivingService.SearchReceivingAsync(SearchFilters);
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
                CreateRec = null;
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
                    // Delete the Rec
                    await ReceivingService.DeleteReceivingAsync(DeleteID.Value);

                    // Get Rec from API
                    ReturnTable = await ReceivingService.SearchReceivingAsync(SearchFilters);
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

        protected async Task EmptyOnly_Change()
        {
            SearchFilters.IsEmpty = !SearchFilters.IsEmpty;
            await Search_Click();
        }
        
        protected async Task TodayOnly_Change()
        {
            SearchFilters.IsToday = !SearchFilters.IsToday;
            await Search_Click();
        }
    }
}