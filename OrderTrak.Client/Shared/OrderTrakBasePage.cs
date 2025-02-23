using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Layout;

namespace OrderTrak.Client.Shared
{
    public partial class OrderTrakBasePage : ComponentBase
    {
        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        [CascadingParameter]
        public MainLayout Layout { get; set; } = default!;

        public bool IsLoading { get; set; }
    }
}
