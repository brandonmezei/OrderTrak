using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Inventory;
using OrderTrak.API.Models.OrderTrakDB;
using OrderTrak.API.Static;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Services.Inventory
{
    public class InventoryService(OrderTrakContext orderTrakContext) : IInventoryService
    {
        private readonly OrderTrakContext DB = orderTrakContext;

        public async Task<PagedTable<InventorySearchReturnDTO>> SearchInventoryAsync(InventorySearchDTO searchQuery)
        {
            // Build base Query
            var query = DB.INV_Stock
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
                    query = query.Where(x =>
                        x.Id.ToString().Contains(filter) ||
                        x.PO_Line.PO_Header.UPL_Project.ProjectCode.Contains(filter) ||
                        x.PO_Line.UPL_PartInfo.PartNumber.Contains(filter) ||
                        x.UPL_Location.LocationNumber.Contains(filter) ||
                        x.PO_Line.PO_Header.PONumber.Contains(filter) ||
                        x.UPL_StockGroup.StockGroupTitle.Contains(filter) ||
                        x.SerialNumber.Contains(filter)
                    );
                }
            }

            // Order Line Filter
            if (searchQuery.OrderLineID.HasValue)
            {
                // Get Order Line
                var orderLine = await DB.ORD_Line
                    .Include(x => x.ORD_Order)
                    .FirstOrDefaultAsync(x => x.FormID == searchQuery.OrderLineID.Value)
                    ?? throw new ValidationException("Order Line not found.");

                query = query.Where(x => x.PO_Line.PartID == orderLine.PartID
                    && x.PO_Line.PO_Header.ProjectID == orderLine.ORD_Order.ProjectID
                    && x.INV_StockStatus.StockStatus == StockStatus.InStock
                    && (!orderLine.POHeaderID.HasValue || x.PO_Line.POHeaderID == orderLine.POHeaderID)
                    && (!orderLine.StockGroupID.HasValue || x.StockGroupID == orderLine.StockGroupID)
                    && (orderLine.SerialNumber == null || x.SerialNumber == orderLine.SerialNumber)
                    );
            }

            // Apply Order By
            query = searchQuery.SortColumn switch
            {
                1 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.Id)
                                        : query.OrderByDescending(x => x.Id),
                2 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.PO_Line.PO_Header.UPL_Project.ProjectCode)
                                        : query.OrderByDescending(x => x.PO_Line.PO_Header.UPL_Project.ProjectCode),
                3 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.PO_Line.UPL_PartInfo.PartNumber)
                                        : query.OrderByDescending(x => x.PO_Line.UPL_PartInfo.PartNumber),
                4 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.PO_Line.UPL_PartInfo.PartDescription)
                                        : query.OrderByDescending(x => x.PO_Line.UPL_PartInfo.PartDescription),
                5 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.UPL_Location.LocationNumber)
                                        : query.OrderByDescending(x => x.UPL_Location.LocationNumber),
                6 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.PO_Line.PO_Header.PONumber)
                                        : query.OrderByDescending(x => x.PO_Line.PO_Header.PONumber),
                7 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.UPL_StockGroup.StockGroupTitle)
                                        : query.OrderByDescending(x => x.UPL_StockGroup.StockGroupTitle),
                8 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.SerialNumber)
                                        : query.OrderByDescending(x => x.SerialNumber),
                9 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.Quantity)
                                        : query.OrderByDescending(x => x.Quantity),
                _ => query.OrderBy(x => x.Id),
            };

            // Apply pagination and projection
            var invList = await query
                .Skip(searchQuery.RecordSize * (searchQuery.Page - 1))
                .Take(searchQuery.RecordSize)
                .AsNoTracking()
                .Select(x => new InventorySearchReturnDTO
                {
                    FormID = x.FormID,
                    BoxID = x.Id,
                    ProjectCode = x.PO_Line.PO_Header.UPL_Project.ProjectCode,
                    PartNumber = x.PO_Line.UPL_PartInfo.PartNumber,
                    PartDescription = x.PO_Line.UPL_PartInfo.PartDescription,
                    Location = x.UPL_Location.LocationNumber,
                    PO = x.PO_Line.PO_Header.PONumber,
                    StockGroup = x.UPL_StockGroup.StockGroupTitle,
                    SerialNumber = x.SerialNumber,
                    Quantity = x.Quantity
                })
                .ToListAsync();

            // Return Object
            return new PagedTable<InventorySearchReturnDTO>
            {
                Data = invList,
                TotalRecords = await query.CountAsync(),
                PageIndex = searchQuery.Page
            };
        }
    }
}
