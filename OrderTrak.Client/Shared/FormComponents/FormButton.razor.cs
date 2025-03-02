using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace OrderTrak.Client.Shared.FormComponents
{
    public partial class FormButton
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; } = default!;

        [Parameter]
        public string? Style { get; set; }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? URL { get; set; }

        [Parameter]
        public EventCallback OnClick { get; set; }

        protected bool IsLoading { get; set; }

        private async Task OnClick_Handler()
        {
            IsLoading = true;

            await OnClick.InvokeAsync();

            await JSRuntime.InvokeVoidAsync("scrollToTop");

            IsLoading = false;

            StateHasChanged();
        }
    }
}