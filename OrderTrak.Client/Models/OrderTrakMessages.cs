namespace OrderTrak.Client.Models
{
    public class OrderTrakMessages
    {
        public string HeaderMessage { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string BulmaLevel { get; set; } = string.Empty;
        public enum MessageType
        {
            Success,
            Warning,
            Error
        }
    }
}
