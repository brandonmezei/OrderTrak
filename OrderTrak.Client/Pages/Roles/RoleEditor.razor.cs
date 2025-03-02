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

        protected int SortOrder { get; set; } = 1;
        protected int SortColumn { get; set; } = 1;

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Role Admin", "Create and edit roles. Add users to roles.");

            try
            {
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

                    RoleToFunctionFromDB = await RoleServices.GetRoleToFunctionByRoleIDAsync(FormID);
                    FilteredRoleToFunctionList = RoleToFunctionFromDB;
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

        protected async Task Save_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (Role != null)
                {
                    await RoleServices.UpdateRoleAsync(MapperService.Map<RoleUpdateDTO>(Role));

                    if(FilteredRoleToFunctionList?.Count > 0)
                    {
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
            Layout.ClearMessages();

            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            try
            {
                if (Role != null)
                {
                    FilteredRoleToFunctionList = RoleToFunctionFromDB;

                    if (FilteredRoleToFunctionList != null && !string.IsNullOrEmpty(RoleToFunctionSearchFilter.SearchText))
                        FilteredRoleToFunctionList = [.. FilteredRoleToFunctionList
                        .Where(
                            p => p.FunctionName.Contains(RoleToFunctionSearchFilter.SearchText, StringComparison.OrdinalIgnoreCase)
                        )
                        ];
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
    }
}