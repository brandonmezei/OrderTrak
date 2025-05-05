using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Filters;

namespace OrderTrak.Client.Shared.DropDowns
{
    public partial class PurchaseOrderDropDown
    {
        [Inject]
        private IDropDownFactoryService DropDownFilterFactory { get; set; } = default!;

        protected List<DropDownFilterDTO> DropDownFilters { get; set; } = [];

        [Parameter]
        public Guid? SelectedValue { get; set; }

        [Parameter]
        public Guid PartID { get; set; }

        [Parameter]
        public Guid ProjectID { get; set; }

        [Parameter]
        public EventCallback<Guid?> SelectedValueChanged { get; set; }

        protected override async Task OnInitializedAsync()
        {
            DropDownFilters = await DropDownFilterFactory.GetPOListGroupAsync(new POListFilterDTO { PartID = PartID, ProjectID = ProjectID } );
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