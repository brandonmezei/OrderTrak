using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Profile;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Profile
{
    public partial class UserAdmin
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private IProfileService ProfileService { get; set; } = default!;

        protected ProfileDTO? Profile { get; set; }

        public bool DeleteUser { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("User Admin", "Edit user information. Approve / Deny users.");

            try
            {
                Profile = await ProfileService.GetUserProfileAsync(FormID);
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

        protected async Task Save_Click()
        {
            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            try
            {
                if (Profile != null)
                {
                    await ProfileService.UpdateUserAdminAsync(MapperService.Map<UserAdminUpdateDTO>(Profile));

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
                IsLoading = false;
            }
        }

        protected void Delete_Toggle()
        {
            Layout.ClearMessages();

            DeleteUser = !DeleteUser;
        }

        protected async Task DeleteConfirm_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (Profile != null)
                {
                    // Delete Customer
                    await ProfileService.DeleteUserAdminAsync(FormID);

                    Navigation.NavigateTo($"/useradmin/search?Delete=true");
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
                DeleteUser = false;
            }
        }
    }
}