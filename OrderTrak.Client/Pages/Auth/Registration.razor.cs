using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Auth;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Auth
{
    public partial class Registration
    {
        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        private IAuthService AuthService { get; set; } = default!;

        public RegisterDTO RegisterModel { get; set; } = new();

        public string? ConfirmPassword { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Welcome to OrderTrak", "Please register below.");

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                Navigation.NavigateTo("/changelog");
            }
        }

        protected async Task Register_Click()
        {
            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            try
            {
                // Check if Passwords Match
                if (!string.IsNullOrEmpty(RegisterModel.Password) && !string.IsNullOrEmpty(ConfirmPassword) && string.Compare(RegisterModel.Password, ConfirmPassword) == 0)
                {
                    await AuthService.Register(RegisterModel);
                    Layout.AddMessage("Registration Successful", MessageType.Success);

                    Navigation.NavigateTo("/login");
                }
                else
                    Layout.AddMessage("Passwords do not match.", MessageType.Error);
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