namespace OrderTrak.Client.Pages.Profile
{
    public partial class UserProfile
    {
        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("User Profile", "Manage user profile and change password.");
        }
    }
}