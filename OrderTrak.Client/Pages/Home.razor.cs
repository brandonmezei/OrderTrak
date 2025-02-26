using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using OrderTrak.Client.Shared;

namespace OrderTrak.Client.Pages
{
    public partial class Home : OrderTrakBasePage
    {
        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                await LocalStorage.RemoveItemAsync("search");
                Navigation.NavigateTo("/changelog");
            }
        }
    }
}