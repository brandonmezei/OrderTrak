using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Models;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Roles;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Roles
{
    public partial class RoleEditor
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private IRoleServices RoleServices { get; set; } = default!;

        protected RoleDTO? Role { get; set; }

        protected List<RoleToFunctionDTO>? FilteredRoleToFunctionList { get; set; }
        protected List<RoleToFunctionDTO>? RoleToFunctionFromDB { get; set; }

        protected TableSearch RoleToFunctionSearchFilter { get; set; } = new();

        protected PagedTableOfRoleToUserReturnDTO? RoleToUserList { get; set; }

        protected RoleToUserSearchDTO SearchFilters { get; set; } = new() { Page = 1, RecordSize = 50, SortOrder = 1, SortColumn = 1 };

        protected int SortOrder { get; set; } = 1;
        protected int SortColumn { get; set; } = 1;

        protected bool IsUserToRoleLoading { get; set; }
        protected bool IsUserToRoleCardLoading { get; set; }
        protected bool AddUserShow { get; set; }
        protected bool DeleteRole { get; set; }

        protected Guid? UserDeleteID { get; set; }
        protected Guid? UserAddID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            // Reset Headers
            Layout.ClearMessages();
            Layout.UpdateHeader("Role Admin", "Create and edit roles. Add users to roles.");

            try
            {
                // Get Role Upper Info
                Role = await RoleServices.GetRoleAsync(FormID);
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
                IsCardLoading = true;
                IsUserToRoleCardLoading = true;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    // Sleep for 500ms to allow the page to render before loading the data
                    await Task.Delay(500);

                    // Get Role Fuctions
                    RoleToFunctionFromDB = await RoleServices.GetRoleToFunctionByRoleIDAsync(FormID);
                    FilteredRoleToFunctionList = RoleToFunctionFromDB;

                    IsCardLoading = false;
                    StateHasChanged();

                    // Sleep for 500ms to allow the page to render before loading the data
                    await Task.Delay(500);

                    // Get Role Users
                    SearchFilters.FormID = FormID;
                    RoleToUserList = await RoleServices.GetUserByRolesAsync(SearchFilters);
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
                    IsUserToRoleCardLoading = false;
                    StateHasChanged();
                }
            }
        }

        protected async Task Save_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (Role != null)
                {
                    // Save Role
                    await RoleServices.UpdateRoleAsync(MapperService.Map<RoleUpdateDTO>(Role));

                    if (FilteredRoleToFunctionList?.Count > 0)
                    {
                        // Save Role to Functions
                        var roletoFunctionUpdate = new RoleUpdateRoleToFunctionDTO
                        {
                            RoleID = Role.FormID
                        };

                        foreach (var roleToFunction in FilteredRoleToFunctionList)
                            roletoFunctionUpdate.UpdateList.Add(MapperService.Map<RoleUpdateRoleToFunctionListDTO>(roleToFunction));


                        await RoleServices.UpdateRoleToFunctionAsync(roletoFunctionUpdate);
                    }

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
        }

        protected void RoleSearch_Click()
        {
            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            try
            {
                if (Role != null)
                {
                    // Local Search for Role to Functions
                    FilteredRoleToFunctionList = RoleToFunctionFromDB;

                    if (!string.IsNullOrEmpty(RoleToFunctionSearchFilter.SearchText) && FilteredRoleToFunctionList != null)
                    {
                        var searchFilter = RoleToFunctionSearchFilter.SearchText
                           .Split(',')
                           .Select(x => x.Trim())
                           .Where(x => !string.IsNullOrEmpty(x))
                           .ToList();

                        var query = FilteredRoleToFunctionList
                            .AsQueryable();

                        foreach (var filter in searchFilter)
                        {
                            query = query.Where(
                                 x => x.FunctionName.Contains(filter, StringComparison.OrdinalIgnoreCase)
                            );
                        }

                        FilteredRoleToFunctionList = [.. query];
                    }
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
                StateHasChanged();
            }
        }

        protected void SortSwitch_Click(int column)
        {
            Layout.ClearMessages();

            SortColumn = column;

            if (FilteredRoleToFunctionList != null)
            {
                // Local Search Role To Function
                if (FilteredRoleToFunctionList.Count == 0)
                    return;

                SortOrder = SortOrder == 1 ? 2 : 1;

                switch (SortOrder)
                {
                    case 1:
                        switch (column)
                        {
                            case 1:
                                FilteredRoleToFunctionList = [.. FilteredRoleToFunctionList.OrderBy(x => x.FunctionName)];
                                break;
                            case 2:
                                FilteredRoleToFunctionList = [.. FilteredRoleToFunctionList.OrderBy(x => x.CanAccess)];
                                break;
                        }
                        break;

                    case 2:
                        switch (column)
                        {
                            case 1:
                                FilteredRoleToFunctionList = [.. FilteredRoleToFunctionList.OrderByDescending(x => x.FunctionName)];
                                break;
                            case 2:
                                FilteredRoleToFunctionList = [.. FilteredRoleToFunctionList.OrderByDescending(x => x.CanAccess)];
                                break;
                        }
                        break;
                    default:
                        FilteredRoleToFunctionList = [.. FilteredRoleToFunctionList.OrderBy(x => x.FunctionName)];
                        break;
                }
            }
        }

        protected async Task UserToRolesSearch_Click()
        {
            if (IsUserToRoleLoading)
                return;

            Layout.ClearMessages();

            IsUserToRoleLoading = true;

            try
            {
                SearchFilters.Page = 1;

                // Get Role Users from API
                RoleToUserList = await RoleServices.GetUserByRolesAsync(SearchFilters);

                if (RoleToUserList?.TotalRecords == 0)
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
                IsUserToRoleLoading = false;
            }
        }

        protected async Task PageSwitch_Click(int page)
        {
            SearchFilters.Page = page;

            try
            {
                // Get Role Users from API
                RoleToUserList = await RoleServices.GetUserByRolesAsync(SearchFilters);

                if (RoleToUserList?.TotalRecords == 0)
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

        protected async Task SortSwitchRoleToUser_Click(int column)
        {
            SearchFilters.SortColumn = column;
            SearchFilters.SortOrder = SearchFilters.SortOrder == 1 ? 2 : 1;

            try
            {
                // Get Role Users from API and sort
                RoleToUserList = await RoleServices.GetUserByRolesAsync(SearchFilters);

                if (RoleToUserList?.TotalRecords == 0)
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

        protected void UserDelete_Toggle(Guid? FormID)
        {
            UserDeleteID = FormID;
        }

        protected async Task DeleteUserFromRole_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (UserDeleteID.HasValue)
                {
                    // Delete user from role
                    await RoleServices.DeleteUserFromRoleAsync(new RoleToUserSelectDTO { RoleID = FormID, UserID = UserDeleteID.Value });

                    Layout.AddMessage(Messages.DeleteSuccessful, MessageType.Success);

                    SearchFilters.Page = 1;
                    RoleToUserList = await RoleServices.GetUserByRolesAsync(SearchFilters);
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
                UserDeleteID = null;
            }
        }

        protected void AddUser_Toggle()
        {
            AddUserShow = !AddUserShow;
        }

        protected void AddUserDropDown_Change(Guid? FormID)
        {
            UserAddID = FormID;
        }

        protected async Task AddUserToRole_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (UserAddID.HasValue)
                {
                    // Add user to role
                    await RoleServices.AddUserToRoleAsync(new RoleToUserSelectDTO { RoleID = FormID, UserID = UserAddID.Value });
                    Layout.AddMessage(Messages.SaveSuccesful, MessageType.Success);

                    SearchFilters.Page = 1;
                    RoleToUserList = await RoleServices.GetUserByRolesAsync(SearchFilters);
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
                AddUserShow = false;
                UserAddID = null;
            }
        }

        protected void DeleteRole_Toggle()
        {
            DeleteRole = !DeleteRole;
        }

        protected async Task DeleteRoleConfirm_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (Role != null)
                {
                    // Delete Role
                    await RoleServices.DeleteRoleAsync(Role.FormID);
                    Layout.AddMessage(Messages.DeleteSuccessful, MessageType.Success);

                    Navigation.NavigateTo("/role/search?Delete=true");
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
                DeleteRole = false;
            }
        }
    }
}