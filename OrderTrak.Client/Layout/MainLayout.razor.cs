using Microsoft.AspNetCore.Components;

namespace OrderTrak.Client.Layout
{
    public partial class MainLayout
    {
        protected string HeaderMessage { get; set; } = "Welcome to OrderTrak";
        protected string SubTitle { get; set; } = "Your trusted order management system...";

        public void UpdateHeader(string message, string subtitle)
        {
            HeaderMessage = message;
            SubTitle = subtitle;
            StateHasChanged();
        }
    }
}