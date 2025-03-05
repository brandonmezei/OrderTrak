using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.ChangeLog;
using OrderTrak.API.Models.OrderTrakDB;

namespace OrderTrak.API.Services.ChangeLog
{
    public class ChangeLogService(OrderTrakContext db) : IChangeLogService
    {
        private readonly OrderTrakContext DB = db;

        public async Task<PagedTable<ChangeLogDTO>> GetChangeLogsAsync(SearchQueryDTO searchQuery)
        {
            // Get Change Logs
            var query = DB.SYS_ChangeLog
                .Include(x => x.SYS_ChangeLogDetails)
                .AsQueryable();

            // Apply pagination and projection
            var changeLogList = await query
                .OrderByDescending(x => x.CreateDate)
                .Skip(searchQuery.RecordSize * (searchQuery.Page - 1))
                .Take(searchQuery.RecordSize)
                .AsNoTracking()
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
