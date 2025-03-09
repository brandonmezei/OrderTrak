using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Profile;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Profile
{
    public partial class UserProfile
    {
        [Inject]
        private IProfileService ProfileService { get; set; } = default!;

        protected ProfileDTO? Profile { get; set; }

        protected string? CurrentPassword { get; set; }
        protected string? NewPassword { get; set; }
        protected string? ConfirmPassword { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("User Profile", "Manage user profile and change password.");

            try
            {
                Profile = await ProfileService.GetUserProfileAsync();
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
                    await ProfileService.UpdateProfileAsync(MapperService.Map<ProfileUpdateDTO>(Profile));

                    // Update the local storage with the new name
                    await LocalStorage.SetItemAsync("fullname", $"{Profile.FirstName} {Profile.LastName}");

                    // Do Full Refresh
                    Navigation.NavigateTo("/profile", true);

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

        protected async Task ChangePassword_Click()
        {
            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            try
            {
                if (Profile != null)
                {

                    // Reload Profile from DB
                    Profile = await ProfileService.GetUserProfileAsync();

                    // Update Password
                    if (!string.IsNullOrEmpty(CurrentPassword) && !string.IsNullOrEmpty(NewPassword) && !string.IsNullOrEmpty(ConfirmPassword) && string.Compare(NewPassword, ConfirmPassword) == 0)
                    {
                        var projectUpdate = MapperService.Map<ProfileUpdateDTO>(Profile);

                        projectUpdate.CurrentPassword = CurrentPassword;
                        projectUpdate.Password = NewPassword;

                        await ProfileService.UpdateProfileAsync(projectUpdate);
                        Layout.AddMessage(Messages.SaveSuccesful, MessageType.Success);
                    }
                    else
                        Layout.AddMessage("Passwords do not match.", MessageType.Error);
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
    }
}