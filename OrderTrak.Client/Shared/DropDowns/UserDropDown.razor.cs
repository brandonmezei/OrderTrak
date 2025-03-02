using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Filters;

namespace OrderTrak.Client.Shared.DropDowns
{
    public partial class UserDropDown
    {
        [Inject]
        private IDropDownFactoryService DropDownFilterFactory { get; set; } = default!;

        protected List<DropDownFilterDTO> DropDownFilters { get; set; } = [];

        [Parameter]
        public bool UnassignedUsersOnly { get; set; }

        [Parameter]
        public Guid? SelectedValue { get; set; }

        [Parameter]
        public EventCallback<Guid?> SelectedValueChanged { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if(UnassignedUsersOnly)
                DropDownFilters = await DropDownFilterFactory.GetUnassignedUsers();
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