using Microsoft.AspNetCore.Components;

namespace OrderTrak.Client.Shared.Order
{
    public partial class OrderTabSwitcher
    {
        [Parameter]
        public int Section { get; set; } = 1;

        [Parameter]
        public Guid? FormID { get; set; }
    }
}