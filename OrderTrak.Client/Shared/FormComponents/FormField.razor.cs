using Microsoft.AspNetCore.Components;

namespace OrderTrak.Client.Shared.FormComponents
{
    public partial class FormField
    {
        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public RenderFragment? Content { get; set; }

        [Parameter]
        public bool Disabled { get; set; }
    }
}