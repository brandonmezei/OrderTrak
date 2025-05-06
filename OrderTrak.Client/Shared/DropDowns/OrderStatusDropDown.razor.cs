using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Filters;

namespace OrderTrak.Client.Shared.DropDowns
{
    public partial class OrderStatusDropDown
    {
        [Inject]
        private IDropDownFactoryService DropDownFilterFactory { get; set; } = default!;

        protected List<DropDownFilterDTO> DropDownFilters { get; set; } = [];

        [Parameter]
        public Guid? SelectedValue { get; set; }

        [Parameter]
        public List<string> IncludeList { get; set; } = [];

        [Parameter]
        public List<string> ExcludeList { get; set; } = [];

        [Parameter]
        public EventCallback<Guid?> SelectedValueChanged { get; set; }

        protected override async Task OnInitializedAsync()
        {
            DropDownFilters = await DropDownFilterFactory.GetOrderStatusListAsync();

            if (IncludeList.Count > 0)
                DropDownFilters = [.. DropDownFilters.Where(x => IncludeList.Contains(x.Label))];

            if (ExcludeList.Count > 0)
                DropDownFilters = [.. DropDownFilters.Where(x => !ExcludeList.Contains(x.Label))];
        }

        private async Task OnSelectedValueChanged(ChangeEventArgs e)
        {
            if (Guid.TryParse(e.Value?.ToString(), out Guid value))
                await SelectedValueChanged.InvokeAsync(value);
            else
                await SelectedValueChanged.InvokeAsync(null);

            StateHasChanged();
        }
    }
}