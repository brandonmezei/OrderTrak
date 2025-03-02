using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Auth;
using OrderTrak.Client.Shared;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Auth
{
    public partial class Login : OrderTrakBasePage
    {
        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        private IAuthService AuthService { get; set; } = default!;

        protected LoginDTO LoginModel { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            // Reset Header
            Layout.ClearMessages();
            Layout.UpdateHeader("Welcome to OrderTrak", "Please login below.");

            // Get Auth State
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            // Redirect if already logged in
            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                Navigation.NavigateTo("/changelog");
            }
        }

        protected async Task Login_Click()
        {
            
            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            try
            {
                // Login and Redirect
                await AuthService.Login(LoginModel);
                Navigation.NavigateTo("/changelog");
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