using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.ChangeLog
{
    public class ChangeLogService : IChangeLogService
    {
        private readonly IClient _apiService;

        public ChangeLogService(IClient apiService)
        {
            _apiService = apiService;
        }

        public async Task<PagedTableOfChangeLogDTO> GetChangeLogsAsync(SearchQueryDTO searchQuery)
        {
            return await _apiService.GetChangeLogsAsync(searchQuery);
        }
    }
}
