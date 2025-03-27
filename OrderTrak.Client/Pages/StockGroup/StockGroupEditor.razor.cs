using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.StockGroup;
using OrderTrak.Client.Statics;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.StockGroup
{
    public partial class StockGroupEditor
    {
        [Parameter]
        public Guid FormID { get; set; }

        [Inject]
        private IStockGroupService StockGroupService { get; set; } = default!;

        protected StockGroupDTO? StockGroup { get; set; }

        public bool DeleteStockGroup { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Stock Group Admin", "Create and edit stock groups.");

            try
            {
                StockGroup = await StockGroupService.GetStockGroupAsync(FormID);
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
                if (StockGroup != null)
                {
                    await StockGroupService.UpdateStockGroupAsync(MapperService.Map<StockGroupUpdateDTO>(StockGroup));

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

            DeleteStockGroup = !DeleteStockGroup;
        }

        protected async Task DeleteConfirm_Click()
        {
            Layout.ClearMessages();

            try
            {
                if (StockGroup != null)
                {
                    // Delete Location
                    await StockGroupService.DeleteStockGroupAsync(StockGroup.FormID);

                    Navigation.NavigateTo($"/stockgroup/search?Delete=true");
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
                DeleteStockGroup = false;
            }
        }
    }
}