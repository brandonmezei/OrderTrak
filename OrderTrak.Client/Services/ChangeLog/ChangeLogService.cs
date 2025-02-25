using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.ChangeLog
{
    public class ChangeLogService(IClient client) : IChangeLogService
    {
        private readonly IClient ApiService = client;

        public async Task<PagedTableOfChangeLogDTO> GetChangeLogsAsync(SearchQueryDTO searchQuery)
        {
            return await ApiService.GetChangeLogsAsync(searchQuery);
        }
    }
}
