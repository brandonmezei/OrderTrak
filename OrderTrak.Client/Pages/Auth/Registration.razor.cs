using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Auth;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Auth
{
    public partial class Registration
    {
        [Inject]
        private IAuthService _authService { get; set; } = default!;

        public RegisterDTO RegisterModel { get; set; } = new();

        protected override void OnInitialized()
        {
            Layout.UpdateHeader("Welcome to OrderTrak", "Please Register Below...");
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