using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.ChangeLog;

namespace OrderTrak.API.Services.ChangeLog
{
    public interface IChangeLogService
    {
        Task<PagedTable<ChangeLogDTO>> GetChangeLogsAsync(SearchQueryDTO searchQuery);
    }
}
