using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Layout;

namespace OrderTrak.Client.Shared
{
    public partial class OrderTrakBasePage : ComponentBase
    {
        [CascadingParameter]
        public MainLayout Layout { get; set; } = default!;
    }
}
