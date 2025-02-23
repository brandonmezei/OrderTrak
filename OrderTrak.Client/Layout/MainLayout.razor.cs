using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using OrderTrak.Client.Models;
using System.Security.Claims;

namespace OrderTrak.Client.Layout
{
    public partial class MainLayout
    {
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; } = default!;

        protected string HeaderMessage { get; set; } = "Welcome to OrderTrak";
        protected string SubTitle { get; set; } = "Your trusted order management system...";

        protected string? UserName { get; set; }

        protected List<OrderTrakMessages> Messages { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            var authState = await authenticationStateTask;
            var user = authState.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                UserName = user.FindFirst(c => c.Type == "Full Name")?.Value;
            }
        }

        public void UpdateHeader(string message, string subtitle)
        {
            HeaderMessage = message;
            SubTitle = subtitle;
            StateHasChanged();
        }

        public void AddMessage(string message, OrderTrakMessages.MessageType type)
        {
            // Parse Message to ApiExceptionReturn
            try
            {
                var messageObj = JsonConvert.DeserializeObject<ApiExceptionReturn>(message);

                // Get Api Exceptions
                if (messageObj != null && messageObj?.Errors?.Count > 0)
                {
                    foreach (var error in messageObj.Errors)
                        AddMessage($"{string.Join("\n", error.Value)}", type);

                    return;
                }
            }
            catch { }   

            var pushedMessage = new OrderTrakMessages { Text = message };

            switch (type)
            {
                case OrderTrakMessages.MessageType.Success:
                    pushedMessage.BulmaLevel = "is-success";
                    break;
                case OrderTrakMessages.MessageType.Warning:
                    pushedMessage.BulmaLevel = "is-warning";
                    break;
                case OrderTrakMessages.MessageType.Error:
                    pushedMessage.BulmaLevel = "is-danger";
                    break;
                default:
                    pushedMessage.BulmaLevel = "is-info";
                    break;
            }

            Messages.Add(pushedMessage);
            StateHasChanged();
        }

        public void ClearMessages()
        {
            Messages.Clear();
            StateHasChanged();
        }

        public void RemoveMessage(OrderTrakMessages message)
        {
            Messages.Remove(message);
            StateHasChanged();
        }
    }
}