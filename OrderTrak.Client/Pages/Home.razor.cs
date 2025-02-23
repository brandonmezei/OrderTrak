using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using OrderTrak.Client.Shared;

namespace OrderTrak.Client.Pages
{
    public partial class Home : OrderTrakBasePage
    {
        [Inject]
        private AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                Navigation.NavigateTo("/changelog");
            }
        }
    }
}