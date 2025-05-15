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

            // Inventory Filter
            if(searchQuery.InventoryID.HasValue)
                query = query.Where(x => x.FormID == searchQuery.InventoryID);

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

        public async Task<Guid> SplitBoxIDAsync(Guid FormID, int qty)
        {
            // Get the Inv Stock
            var invStock = await DB.INV_Stock
                .FirstOrDefaultAsync(x => x.FormID == FormID)
                ?? throw new ValidationException("Inventory not found.");

            // Check if the quantity is valid
            if (qty <= 0 || qty >= invStock.Quantity)
                throw new ValidationException("Invalid quantity.");

            // Create a new Inv Stock
            var newStock = new INV_Stock
            {
                ReceiptID = invStock.ReceiptID,
                POLineID = invStock.POLineID,
                StatusID = invStock.StatusID,
                StockGroupID = invStock.StockGroupID,
                LocationID = invStock.LocationID,
                Quantity = qty,
                SerialNumber = invStock.SerialNumber,
                AssetTag = invStock.AssetTag,
                UDF1 = invStock.UDF1,
                UDF2 = invStock.UDF2,
                UDF3 = invStock.UDF3,
                UDF4 = invStock.UDF4,
                UDF5 = invStock.UDF5,
                UDF6 = invStock.UDF6,
                UDF7 = invStock.UDF7,
                UDF8 = invStock.UDF8,
                UDF9 = invStock.UDF9,
                UDF10 = invStock.UDF10
            };

            // Add the new stock to the database
            await DB.INV_Stock.AddAsync(newStock);

            // Update the original stock quantity
            invStock.Quantity -= qty;

            // Save changes to the database
            await DB.SaveChangesAsync();

            // Return the new stock ID
            return newStock.FormID;
        }
    }
}
