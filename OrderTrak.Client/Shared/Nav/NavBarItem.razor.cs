using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace OrderTrak.Client.Shared.Nav
{
    public partial class NavBarItem
    {

        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private ILocalStorageService LocalStorageService { get; set; } = default!;

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public string Label { get; set; } = string.Empty;

        [Parameter]
        public string URL { get; set; } = string.Empty;

        protected void NavigateTo()
        {
            if (!string.IsNullOrEmpty(URL))
            {
                LocalStorageService.RemoveItemAsync("search");
                Navigation.NavigateTo(URL);
            }
        }
    }
}