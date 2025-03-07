using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace OrderTrak.Client.Shared.Nav
{
    public partial class NavBar
    {
        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private ILocalStorageService LocalStorageService { get; set; } = default!;

        protected string? UserName { get; set; }

        protected List<string> Permissions { get; set; } = [];

        protected bool IsSettingOpen { get; set; }
        protected bool IsUserOpen { get; set; }
        protected bool IsWarehouseOpen { get; set; }

        protected override async Task OnInitializedAsync()
        {
            UserName = await LocalStorageService.GetItemAsync<string>("fullname");
            Permissions = await LocalStorageService.GetItemAsync<List<string>>("permissions") ?? [];

            Navigation.LocationChanged += (sender, args) =>
            {
                IsSettingOpen = false;
                IsUserOpen = false;
                IsWarehouseOpen = false;
                StateHasChanged();
            };
        }
        protected void Toggle_Setting()
        {
            IsSettingOpen = !IsSettingOpen;
            IsWarehouseOpen = false;
            IsUserOpen = false;
        }

        protected void Toggle_Warehouse()
        {
            IsWarehouseOpen = !IsWarehouseOpen;
            IsSettingOpen = false;
            IsUserOpen = false;
        }

        protected void Toggle_User()
        {
            IsUserOpen = !IsUserOpen;
            IsSettingOpen = false;
            IsWarehouseOpen = false;
        }
    }
}