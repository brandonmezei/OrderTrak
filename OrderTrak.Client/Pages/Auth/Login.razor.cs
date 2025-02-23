using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Auth;
using OrderTrak.Client.Shared;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Auth
{
    public partial class Login : OrderTrakBasePage
    {
        [Inject] 
        private IAuthService AuthService { get; set; } = default!;

        protected LoginDTO LoginModel { get; set; } = new();

        protected override void OnInitialized()
        {
            Layout.UpdateHeader("Welcome to OrderTrak", "Please Login Below...");
        }

        protected async Task LoginUser()
        {
            Layout.ClearMessages();

            try
            {
                await AuthService.Login(LoginModel);
            }
            catch(ApiException ex)
            {
                Layout.AddMessage(ex.Response, MessageType.Error);
            }
            catch(Exception ex)
            {
                Layout.AddMessage(ex.Message, MessageType.Error);
            }
        }
    }
}