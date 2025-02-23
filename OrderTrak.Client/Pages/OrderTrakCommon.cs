using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Layout;

namespace OrderTrak.Client.Pages
{
    public class OrderTrakCommon : ComponentBase
    {
        [CascadingParameter]
        public MainLayout MainLayout { get; set; } = default!;
    }
}
