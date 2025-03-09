using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.StockGroup;
using OrderTrak.API.Models.OrderTrakDB;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Services.StockGroup
{
    public class StockGroupService(OrderTrakContext orderTrakContext) : IStockGroupService
    {
        private readonly OrderTrakContext DB = orderTrakContext;

        public async Task<Guid> CreateStockGroupAsync(StockGroupCreateDTO stockGroupCreateDTO)
        {
            // Check if stockgrouptitle already exists
            if (await DB.UPL_StockGroup.AnyAsync(x => x.StockGroupTitle == stockGroupCreateDTO.StockGroupTitle))
                throw new ValidationException("Stock Group already exists");

            // Create new stockgroup
            var newStockGroup = new UPL_StockGroup
            {
                StockGroupTitle = stockGroupCreateDTO.StockGroupTitle
            };

            // Save
            await DB.UPL_StockGroup.AddAsync(newStockGroup);
            await DB.SaveChangesAsync();

            return newStockGroup.FormID;
        }

        public async Task DeleteStockGroupAsync(Guid stockGroupID)
        {
            // Get StockGroup By ID
            var stockGroup = await DB.UPL_StockGroup
                .FirstOrDefaultAsync(x => x.FormID == stockGroupID)
                ?? throw new ValidationException("Stock Group not found");

            // Soft Delete
            stockGroup.IsDelete = true;

            await DB.SaveChangesAsync();
        }

        public async Task<StockGroupDTO> GetStockGroupAsync(Guid stockGroupID)
        {
            return await DB.UPL_StockGroup
                .Where(x => x.FormID == stockGroupID)
                .Select(x => new StockGroupDTO
                {
                    FormID = x.FormID,
                    StockGroupTitle = x.StockGroupTitle
                })
                .FirstOrDefaultAsync()
                ?? throw new ValidationException("Stock Group not found");
        }

        public async Task<PagedTable<StockGroupSearchReturnDTO>> SearchStockGroupAsync(SearchQueryDTO searchQuery)
        {
            // Get StockGroup Query
            var query = DB.UPL_StockGroup
                .AsQueryable();

            // Filters
            if (!string.IsNullOrEmpty(searchQuery.SearchFilter))
            {
                var searchFilter = searchQuery.SearchFilter
                    .Split(',')
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

                foreach (var filter in searchFilter)
                {
                    query = query.Where(x => x.StockGroupTitle.Contains(filter));
                }
            }

            // Apply Order By
            switch (searchQuery.SortColumn)
            {
                case 1:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.StockGroupTitle)
                        : query.OrderByDescending(x => x.StockGroupTitle);
                    break;
                default:
                    query = query.OrderBy(x => x.Id);
                    break;
            }

            var stockGroupList = await query
                .Skip(searchQuery.RecordSize * (searchQuery.Page - 1))
                .Take(searchQuery.RecordSize)
                .AsNoTracking()
                .Select(x => new StockGroupSearchReturnDTO
                {
                    FormID = x.FormID,
                    StockGroupTitle = x.StockGroupTitle
                })
                .ToListAsync();

            // Return Object
            return new PagedTable<StockGroupSearchReturnDTO>
            {
                Data = stockGroupList,
                TotalRecords = await query.CountAsync(),
                PageIndex = searchQuery.Page
            };
        }

        public async Task UpdateStockGroupAsync(StockGroupUpdateDTO stockGroupUpdateDTO)
        {
            // Get StockGroup By ID
            var stockGroup = await DB.UPL_StockGroup
                .FirstOrDefaultAsync(x => x.FormID == stockGroupUpdateDTO.FormID)
                ?? throw new ValidationException("Stock Group not found");

            // Check if stockgrouptitle already exists
            if (await DB.UPL_StockGroup.AnyAsync(x => x.StockGroupTitle == stockGroupUpdateDTO.StockGroupTitle && x.FormID != stockGroupUpdateDTO.FormID))
                throw new ValidationException("Stock Group already exists");

            // Update StockGroup
            stockGroup.StockGroupTitle = stockGroupUpdateDTO.StockGroupTitle ?? throw new ValidationException("Stock Group Name is required.");

            // Save
            await DB.SaveChangesAsync();
        }
    }
}
