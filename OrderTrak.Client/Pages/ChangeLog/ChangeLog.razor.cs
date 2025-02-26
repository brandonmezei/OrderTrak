using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.ChangeLog;
using OrderTrak.Client.Shared;
using static OrderTrak.Client.Models.OrderTrakMessages;

namespace OrderTrak.Client.Pages.ChangeLog
{
    public partial class ChangeLog : OrderTrakBasePage
    {
        [Inject]
        private IChangeLogService ChangeLogService { get; set; } = default!;

        private PagedTableOfChangeLogDTO? ChangeLogs { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.ClearMessages();
            Layout.UpdateHeader("Welcome to OrderTrak", "Here's what's new...");

            try
            {
                ChangeLogs = await ChangeLogService.GetChangeLogsAsync(new SearchQueryDTO
                {
                    Page = 1,
                    RecordSize = 3
                });
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
    }
}