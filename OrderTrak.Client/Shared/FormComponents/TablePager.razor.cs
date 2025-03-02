using Microsoft.AspNetCore.Components;

namespace OrderTrak.Client.Shared.FormComponents
{
    public partial class TablePager
    {
        [Parameter]
        public int PageIndex { get; set; }

        [Parameter]
        public int TotalRecordCount { get; set; }

        [Parameter]
        public int PageSize { get; set; }

        [Parameter]
        public EventCallback<int> OnPageChanged { get; set; }

        protected int TotalPages { get; set; } = 1;

        protected override void OnInitialized()
        {
            TotalPages = TotalRecordCount % PageSize == 0
            ? TotalRecordCount / PageSize
            : (TotalRecordCount / PageSize) + 1;
        }

        protected async Task OnPage_Click(int page)
        {
            page = page < 1 ? 1 : page;
            page = page > TotalPages ? TotalPages : page;

            await OnPageChanged.InvokeAsync(page);

            StateHasChanged();
        }
    }
}