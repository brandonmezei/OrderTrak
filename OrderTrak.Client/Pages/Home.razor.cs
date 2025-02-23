using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Layout;
using System.Runtime.CompilerServices;

namespace OrderTrak.Client.Pages
{
    public partial class Home
    {
        [CascadingParameter]
        public MainLayout MainLayout { get; set; } = default!;

        protected override void OnInitialized()
        {
            MainLayout.UpdateHeader("Welcome to OrderTrak.", "Test Update.");
        }
    }
}