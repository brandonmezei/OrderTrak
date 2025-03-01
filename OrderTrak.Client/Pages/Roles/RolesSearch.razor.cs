using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Roles;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Roles
{
    public partial class RolesSearch
    {
        [Inject]
        private IRoleServices RoleServices { get; set; } = default!;

        [SupplyParameterFromQuery]
        public bool Delete { get; set; }

        protected RoleSearchDTO SearchFilters { get; set; } = new() { Page = 1, RecordSize = 50, SortOrder = 1, SortColumn = 1 };

        protected PagedTableOfRoleSearchReturnDTO? ReturnTable;

        protected RoleCreateDTO? CreateRole { get; set; }
        public Guid? DeleteID { get; set; }

        protected override void OnInitialized()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Role Admin", "Create and edit roles. Add users to roles.");

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

                    SearchFilters = await LocalStorage.GetItemAsync<RoleSearchDTO>("search") ?? SearchFilters;
                    ReturnTable = await RoleServices.SearchRolesAsync(SearchFilters);
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

                ReturnTable = await RoleServices.SearchRolesAsync(SearchFilters);

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

                ReturnTable = await RoleServices.SearchRolesAsync(SearchFilters);

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

                ReturnTable = await RoleServices.SearchRolesAsync(SearchFilters);

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

        protected async Task EmptyRole_Change()
        {
            SearchFilters.EmptyOnly = !SearchFilters.EmptyOnly;
            await Search_Click();
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
                    await RoleServices.DeleteRoleAsync(DeleteID.Value);

                    // Reload Customer List
                    ReturnTable = await RoleServices.SearchRolesAsync(SearchFilters);
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

        protected void CreateRole_Toggle()
        {
            if (CreateRole == null)
                CreateRole = new();
            else
                CreateRole = null;
        }

        protected async Task CreateRole_Submit()
        {
            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            try
            {
                if (CreateRole != null)
                {
                    await RoleServices.CreateRoleAsync(CreateRole);

                    // Reload Customer List
                    ReturnTable = await RoleServices.SearchRolesAsync(SearchFilters);
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
                CreateRole = null;
                IsLoading = false;
            }
        }
    }
}