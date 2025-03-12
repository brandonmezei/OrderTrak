using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Filters;

namespace OrderTrak.Client.Shared.DropDowns
{
    public partial class CustomerProjectDropDown
    {
        [Inject]
        private IDropDownFactoryService DropDownFilterFactory { get; set; } = default!;

        protected List<DropDownFilterDTO> CustomerDropDownFilters { get; set; } = [];
        protected List<DropDownFilterDTO> ProjectDropDownFilters { get; set; } = [];

        [Parameter]
        public Guid? CustomerID { get; set; }

        [Parameter]
        public Guid? ProjectID { get; set; }

        [Parameter]
        public EventCallback<Guid?> CustomerSelectedValueChanged { get; set; }

        [Parameter]
        public EventCallback<Guid?> ProjectSelectedValueChanged { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CustomerDropDownFilters = await DropDownFilterFactory.GetCustomersAsync();

            if (CustomerID.HasValue)
                ProjectDropDownFilters = await DropDownFilterFactory.GetProjectsAsync(CustomerID.Value);
        }

        private async Task CustomerSelectedValue_Changed(ChangeEventArgs e)
        {
            if (Guid.TryParse(e.Value?.ToString(), out Guid value))
            {
                await CustomerSelectedValueChanged.InvokeAsync(value);

                ProjectDropDownFilters = await DropDownFilterFactory.GetProjectsAsync(value);
            }
            else
            {
                await CustomerSelectedValueChanged.InvokeAsync(null);
                ProjectDropDownFilters = [];
            }

            StateHasChanged();
        }

        private async Task ProjectSelectedValue_Changed(ChangeEventArgs e)
        {
            if (Guid.TryParse(e.Value?.ToString(), out Guid value))
            {
                await ProjectSelectedValueChanged.InvokeAsync(value);
            }
            else
                await ProjectSelectedValueChanged.InvokeAsync(null);

            StateHasChanged();
        }
    }
}