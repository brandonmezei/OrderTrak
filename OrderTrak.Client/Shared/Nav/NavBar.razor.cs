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

        protected override async Task OnInitializedAsync()
        {
            UserName = await LocalStorageService.GetItemAsync<string>("fullname");
            Permissions = await LocalStorageService.GetItemAsync<List<string>>("permissions") ?? [];

            Navigation.LocationChanged += (sender, args) =>
            {
                IsSettingOpen = false;
                IsUserOpen = false;
                StateHasChanged();
            };
        }

        protected void Toggle_Setting()
        {
            IsSettingOpen = !IsSettingOpen;
        }

        protected void Toggle_User()
        {
            IsUserOpen = !IsUserOpen;
        }
    }
}