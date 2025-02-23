using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.ChangeLog
{
    public interface IChangeLogService
    {
        Task<PagedTableOfChangeLogDTO> GetChangeLogsAsync(SearchQueryDTO searchQuery);
    }
}
