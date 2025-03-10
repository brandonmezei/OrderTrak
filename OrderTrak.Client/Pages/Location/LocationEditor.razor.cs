using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Location;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Location
{
    public partial class LocationEditor
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private ILocationService LocationService { get; set; } = default!;

        protected LocationDTO? Location { get; set; }

        public bool DeleteLocation { get; set; }

        public readonly List<string> UOMList = [
            UOM.Feet,
            UOM.Inches
        ];

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Location Admin", "Create and edit locations.");

            try
            {
                Location = await LocationService.GetLocationAsync(FormID);
            }
            catch (ApiException ex)
            {
                Layout.AddMessage(ex.Response, MessageType.Error);
            }
            catch (Exception ex)
            {
                Layout.AddMessage(ex.Message, MessageType.Error);
            }
        }

        protected async Task Save_Click()
        {
            if (IsLoading)
                return;

            Layout.ClearMessages();

            IsLoading = true;

            try
            {
                if (Location != null)
                {
                    await LocationService.UpdateLocationAsync(MapperService.Map<LocationUpdateDTO>(Location));

                    Layout.AddMessage(Messages.SaveSuccesful, MessageType.Success);
                }
            }
            catch (ApiException ex)
            {
                Layout.AddMessage(ex.Response, MessageType.Error);
            }
            catch (Exception ex)
            {
                Layout.AddMessage(ex.Message, MessageType.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected void Delete_Toggle()
        {
            Layout.ClearMessages();

            DeleteLocation = !DeleteLocation;
        }

        protected async Task DeleteConfirm_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (Location != null)
                {
                    // Delete Location
                    await LocationService.DeleteLocationAsync(FormID);

                    Navigation.NavigateTo($"/location/search?Delete=true");
                }
            }
            catch (ApiException ex)
            {
                Layout.AddMessage(ex.Response, MessageType.Error);
            }
            catch (Exception ex)
            {
                Layout.AddMessage(ex.Message, MessageType.Error);
            }
            finally
            {
                DeleteLocation = false;
            }
        }

        protected void UOMDropDown_Change(Guid? FormID)
        {
            if (Location != null && FormID.HasValue)
                Location.Uomid = FormID.Value;
        }
    }
}