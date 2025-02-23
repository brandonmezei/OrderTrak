using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Auth;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Auth
{
    public partial class Registration
    {
        [Inject]
        private AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

        [Inject]
        private IAuthService _authService { get; set; } = default!;

        public RegisterDTO RegisterModel { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            Layout.UpdateHeader("Welcome to OrderTrak", "Please register below.");

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                Navigation.NavigateTo("/changelog");
            }
        }

        protected async Task RegisterUser()
        {
            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            try
            {
                await _authService.Register(RegisterModel);
                Layout.AddMessage("Registration Successful", MessageType.Success);

                Navigation.NavigateTo("/login");
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