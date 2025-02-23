using OrderTrak.Client.Models;

namespace OrderTrak.Client.Layout
{
    public partial class MainLayout
    {

        protected string HeaderMessage { get; set; } = "Welcome to OrderTrak";
        protected string SubTitle { get; set; } = "Your trusted order management system...";

        protected List<OrderTrakMessages> Messages { get; set; } = [];

        public void UpdateHeader(string message, string subtitle)
        {
            HeaderMessage = message;
            SubTitle = subtitle;
            StateHasChanged();
        }

        public void AddMessage(string message, OrderTrakMessages.MessageType type)
        {
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