using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.ChangeLog;
using OrderTrak.API.Models.OrderTrakDB;

namespace OrderTrak.API.Services.ChangeLog
{
    public class ChangeLogService : IChangeLogService
    {
        private readonly OrderTrakContext _orderTrakContext;

        public ChangeLogService(OrderTrakContext db)
        {
            _orderTrakContext = db;
        }

        public async Task<PagedTable<ChangeLogDTO>> GetChangeLogsAsync(SearchQueryDTO searchQuery)
        {
            // Get Change Logs
            var query = _orderTrakContext.SYS_ChangeLog
                .Include(x => x.SYS_ChangeLogDetails)
                .OrderByDescending(x => x.CreateDate);

            // Apply pagination and projection
            var changeLogList = await query
                .Skip(searchQuery.RecordSize * (searchQuery.Page - 1))
                .Take(searchQuery.RecordSize)
                .Select(x => new ChangeLogDTO
                {
                    RollOutDate = x.CreateDate,
                    ChangeLogDetails = x.SYS_ChangeLogDetails.Select(i => new ChangeLogDetailsDTO
                    {
                        TicketID = i.TicketID,
                        TicketInfo = i.TicketInfo
                    }).ToList()
                })
                .ToListAsync();


            // Return Object
            return new PagedTable<ChangeLogDTO>
            {
                Data = changeLogList,
                TotalRecords = await query.CountAsync(),
                PageIndex = searchQuery.Page
            };
        }
    }
}
