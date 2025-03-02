using Microsoft.AspNetCore.Components;

namespace OrderTrak.Client.Shared.FormComponents
{
    public partial class ModalPopup
    {
        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public RenderFragment? Content { get; set; }

        [Parameter]
        public RenderFragment? FooterButtons { get; set; }

        [Parameter]
        public EventCallback OnClose { get; set; }


        private async Task OnClose_Handler()
        {
            await OnClose.InvokeAsync();

            StateHasChanged();
        }
    }
}