using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.Parts;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.Parts
{
    public partial class PartEditor
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private IPartService PartService { get; set; } = default!;

        protected PartDTO? Part { get; set; }

        public bool DeletePart { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Part Admin", "Create and edit parts.");

            try
            {
                Part = await PartService.GetPartAsync(FormID);
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
                if (Part != null)
                {
                    await PartService.UpdatePartAsync(MapperService.Map<PartUpdateDTO>(Part));

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

            DeletePart = !DeletePart;
        }

        protected async Task DeleteConfirm_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (Part != null)
                {
                    // Delete Part
                    await PartService.DeletePartAsync(FormID);

                    Navigation.NavigateTo($"/part/search?Delete=true");
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
                DeletePart = false;
            }
        }

        protected void UOMDropDown_Change(Guid? FormID)
        {
            if(Part != null && FormID.HasValue)
                Part.Uomid = FormID.Value;
        }
    }
}