using Newtonsoft.Json;
using OrderTrak.Client.Models;

namespace OrderTrak.Client.Layout
{
    public partial class MainLayout
    {
        protected string HeaderMessage { get; set; } = "Welcome to OrderTrak";
        protected string SubTitle { get; set; } = "Your trusted order management system...";

        public List<OrderTrakMessages> Messages { get; set; } = [];

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

            if (string.IsNullOrEmpty(message) && type == OrderTrakMessages.MessageType.Error)
                message = "An unknown error occurred. You may not be authorized to use this resource. Please try again.";

            var pushedMessage = new OrderTrakMessages { Text = message };


            switch (type)
            {
                case OrderTrakMessages.MessageType.Success:
                    pushedMessage.HeaderMessage = "Success";
                    pushedMessage.BulmaLevel = "is-success";
                    break;
                case OrderTrakMessages.MessageType.Warning:
                    pushedMessage.HeaderMessage = "Warning";
                    pushedMessage.BulmaLevel = "is-warning";
                    break;
                case OrderTrakMessages.MessageType.Error:
                    pushedMessage.HeaderMessage = "Error";
                    pushedMessage.BulmaLevel = "is-danger";
                    break;
                default:
                    pushedMessage.HeaderMessage = "Infomation";
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