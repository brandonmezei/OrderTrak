using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace OrderTrak.Client.Shared.Nav
{
    public partial class NavBar
    {
        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private ILocalStorageService LocalStorageService { get; set; } = default!;

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        protected string? UserName { get; set; }

        protected List<string> Permissions { get; set; } = [];

        protected bool IsSettingOpen { get; set; }
        protected bool IsUserOpen { get; set; }
        protected bool IsWarehouseOpen { get; set; }
        protected bool IsOrdersOpen { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();

            UserName = await LocalStorageService.GetItemAsync<string>("fullname");
            Permissions = await LocalStorageService.GetItemAsync<List<string>>("permissions") ?? [];

            Navigation.LocationChanged += (sender, args) =>
            {
                IsSettingOpen = false;
                IsUserOpen = false;
                IsWarehouseOpen = false;
                IsOrdersOpen = false;
                StateHasChanged();
            };
        }
        protected void Toggle_Setting()
        {
            IsSettingOpen = !IsSettingOpen;
            IsWarehouseOpen = false;
            IsUserOpen = false;
            IsOrdersOpen = false;
        }

        protected void Toggle_Warehouse()
        {
            IsWarehouseOpen = !IsWarehouseOpen;
            IsSettingOpen = false;
            IsUserOpen = false;
            IsOrdersOpen = false;
        }

        protected void Toggle_User()
        {
            IsUserOpen = !IsUserOpen;
            IsSettingOpen = false;
            IsWarehouseOpen = false;
            IsOrdersOpen = false;
        }

        protected void Toggle_Orders()
        {
            IsOrdersOpen = !IsOrdersOpen;
            IsSettingOpen = false;
            IsWarehouseOpen = false;
            IsUserOpen = false;
        }
    }
}