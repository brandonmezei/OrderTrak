using Microsoft.AspNetCore.Components;

namespace OrderTrak.Client.Shared.FormComponents
{
    public partial class CardComponent
    {
        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? RightTitle { get; set; }

        [Parameter]
        public RenderFragment? CardBody { get; set; }

        [Parameter]
        public RenderFragment? Footer { get; set; }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public bool IsCollapsed { get; set; }

        [Parameter]
        public bool IsCardLoading { get; set; }

        public void Collase_Toggle()
        {
            IsCollapsed = !IsCollapsed;

            StateHasChanged();
        }

    }
}