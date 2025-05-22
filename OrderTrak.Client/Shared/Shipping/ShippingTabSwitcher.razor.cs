using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace OrderTrak.Client.Shared.Shipping
{
    public partial class ShippingTabSwitcher
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; } = default!;

        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        [Parameter]
        public int Section { get; set; } = 1;

        [Parameter]
        public Guid? FormID { get; set; }

        private void OnClick_Handler(int redirect)
        {
            switch (redirect)
            {
                case 1:
                    Navigation.NavigateTo($"/shipping/section1/{FormID}");
                    break;
                case 2:
                    Navigation.NavigateTo($"/shipping/section2/{FormID}");
                    break;
                case 3:
                    Navigation.NavigateTo($"/shipping/section3/{FormID}");
                    break;
                default:
                    break;
            }
        }
    }
}