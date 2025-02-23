using System.Runtime.CompilerServices;

namespace OrderTrak.Client.Pages
{
    public partial class Home
    {

        protected override void OnInitialized()
        {
            MainLayout.HeaderMessage = "Welcome";
        }
    }
}