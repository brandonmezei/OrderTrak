using Microsoft.AspNetCore.Components;
using OrderTrak.Client.Services.API;
using OrderTrak.Client.Services.ChangeLog;
using OrderTrak.Client.Shared;

namespace OrderTrak.Client.Pages.ChangeLog
{
    public partial class ChangeLog : OrderTrakBasePage
    {
        [Inject]
        private IChangeLogService _changeLogService { get; set; } = default!;

        private PagedTableOfChangeLogDTO? ChangeLogs { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Layout.UpdateHeader("Welcome to OrderTrak", "Here's what's new...");

            ChangeLogs = await _changeLogService.GetChangeLogsAsync(new SearchQueryDTO
            {
                Page = 1,
                RecordSize = 3
            });
        }
    }
}