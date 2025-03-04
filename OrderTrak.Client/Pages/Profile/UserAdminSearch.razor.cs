using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Profile;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Profile
{
    public partial class UserAdminSearch
    {

        [Inject]
        private IProfileService ProfileService { get; set; } = default!;

        [SupplyParameterFromQuery]
        public bool Delete { get; set; }

        protected SearchQueryDTO SearchFilters { get; set; } = new() { Page = 1, RecordSize = 50, SortOrder = 1, SortColumn = 1 };

        protected PagedTableOfProfileDTO? ReturnTable;

        public Guid? DeleteID { get; set; }

        protected override void OnInitialized()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("User Admin", "Edit user information. Approve / Deny users.");

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

                    SearchFilters = await LocalStorage.GetItemAsync<SearchQueryDTO>("search") ?? SearchFilters;
                    ReturnTable = await ProfileService.SearchUserProfileAsync(SearchFilters);
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

                await LocalStorage.SetItemAsync("search", SearchFilters);

                ReturnTable = await ProfileService.SearchUserProfileAsync(SearchFilters);

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
                await LocalStorage.SetItemAsync("search", SearchFilters);

                ReturnTable = await ProfileService.SearchUserProfileAsync(SearchFilters);

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
                await LocalStorage.SetItemAsync("search", SearchFilters);

                ReturnTable = await ProfileService.SearchUserProfileAsync(SearchFilters);

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
                    // Delete Role and Reload
                    //await RoleServices.DeleteRoleAsync(DeleteID.Value);

                    ReturnTable = await ProfileService.SearchUserProfileAsync(SearchFilters);
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