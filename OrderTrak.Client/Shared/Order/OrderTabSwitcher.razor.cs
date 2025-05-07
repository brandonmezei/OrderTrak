using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using static System.Collections.Specialized.BitVector32;

namespace OrderTrak.Client.Shared.Order
{
    public partial class OrderTabSwitcher
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; } = default!;

        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        [Parameter]
        public int Section { get; set; } = 1;

        [Parameter]
        public Guid? FormID { get; set; }

        [Parameter]
        public EventCallback OnClick { get; set; }

        private async Task OnClick_Handler(int redirect)
        {
            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync();
                StateHasChanged();
            }

            switch (redirect)
            {
                case 1:
                    Navigation.NavigateTo($"/order/section1/{FormID}");
                    break;
                case 2:
                    Navigation.NavigateTo($"/order/section2/{FormID}");
                    break;
                case 3:
                    Navigation.NavigateTo($"/order/section3/{FormID}");
                    break;
                case 4:
                    Navigation.NavigateTo($"/order/section4/{FormID}");
                    break;
                case 5:
                    Navigation.NavigateTo($"/order/section5/{FormID}");
                    break;
                default:
                    break;
            }

        }
    }
}