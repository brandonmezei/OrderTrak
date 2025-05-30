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
                        x.INV_StockStatus.StockStatus.Contains(filter) ||
                        x.SerialNumber.Contains(filter) ||
                        x.AssetTag.Contains(filter) ||
                        x.UDF1.Contains(filter) ||
                        x.UDF2.Contains(filter) ||
                        x.UDF3.Contains(filter) ||
                        x.UDF4.Contains(filter) ||
                        x.UDF5.Contains(filter) ||
                        x.UDF6.Contains(filter) ||
                        x.UDF7.Contains(filter) ||
                        x.UDF8.Contains(filter) ||
                        x.UDF9.Contains(filter) ||
                        x.UDF10.Contains(filter)
                    );
                }
            }

            // Order Line Filter
            if (searchQuery.OrderLineID.HasValue)
            {
                if (searchQuery.ShowPickedOnly)
                {
                    query = query.Where(x => x.ORD_PickList.Any(y => y.ORD_Line.FormID == searchQuery.OrderLineID.Value));
                }
                else
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
            }
            else
            {
                // Filter Out Shipped if not Requested
                if (!searchQuery.ShowShipped)
                    query = query.Where(x => x.INV_StockStatus.StockStatus != StockStatus.Shipped);
            }

            // Inventory Filter
            if (searchQuery.InventoryID.HasValue)
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
                10 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.AssetTag)
                                        : query.OrderByDescending(x => x.AssetTag),
                11 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.UDF1)
                                        : query.OrderByDescending(x => x.UDF1),
                12 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.UDF2)
                                        : query.OrderByDescending(x => x.UDF2),
                13 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.UDF3)
                                        : query.OrderByDescending(x => x.UDF3),
                14 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.UDF4)
                                        : query.OrderByDescending(x => x.UDF4),
                15 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.UDF5)
                                        : query.OrderByDescending(x => x.UDF5),
                16 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.UDF6)
                                        : query.OrderByDescending(x => x.UDF6),
                17 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.UDF7)
                                        : query.OrderByDescending(x => x.UDF7),
                18 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.UDF8)
                                        : query.OrderByDescending(x => x.UDF8),
                19 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.UDF9)
                                        : query.OrderByDescending(x => x.UDF9),
                20 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.UDF10)
                                        : query.OrderByDescending(x => x.UDF10),
                21 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.INV_StockStatus.StockStatus)
                                        : query.OrderByDescending(x => x.INV_StockStatus.StockStatus),
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
                    ProjectID = x.PO_Line.PO_Header.UPL_Project.FormID,
                    StockGroupID = x.UPL_StockGroup.FormID,
                    StatusID = x.INV_StockStatus.FormID,
                    BoxID = x.Id,
                    ProjectCode = x.PO_Line.PO_Header.UPL_Project.ProjectCode,
                    PartNumber = x.PO_Line.UPL_PartInfo.PartNumber,
                    PartDescription = x.PO_Line.UPL_PartInfo.PartDescription,
                    LocationNumber = x.UPL_Location.LocationNumber,
                    Status = x.INV_StockStatus.StockStatus,
                    PO = x.PO_Line.PO_Header.PONumber,
                    StockGroup = x.UPL_StockGroup.StockGroupTitle,
                    SerialNumber = x.SerialNumber,
                    Quantity = x.Quantity,
                    AssetTag = x.AssetTag,
                    UDF1 = x.UDF1,
                    UDF2 = x.UDF2,
                    UDF3 = x.UDF3,
                    UDF4 = x.UDF4,
                    UDF5 = x.UDF5,
                    UDF6 = x.UDF6,
                    UDF7 = x.UDF7,
                    UDF8 = x.UDF8,
                    UDF9 = x.UDF9,
                    UDF10 = x.UDF10,
                    UDFLabel1 = x.PO_Line.PO_Header.UPL_Project.UDF1,
                    UDFLabel2 = x.PO_Line.PO_Header.UPL_Project.UDF2,
                    UDFLabel3 = x.PO_Line.PO_Header.UPL_Project.UDF3,
                    UDFLabel4 = x.PO_Line.PO_Header.UPL_Project.UDF4,
                    UDFLabel5 = x.PO_Line.PO_Header.UPL_Project.UDF5,
                    UDFLabel6 = x.PO_Line.PO_Header.UPL_Project.UDF6,
                    UDFLabel7 = x.PO_Line.PO_Header.UPL_Project.UDF7,
                    UDFLabel8 = x.PO_Line.PO_Header.UPL_Project.UDF8,
                    UDFLabel9 = x.PO_Line.PO_Header.UPL_Project.UDF9,
                    UDFLabel10 = x.PO_Line.PO_Header.UPL_Project.UDF10,
                    CanUpdate = x.INV_StockStatus.StockStatus != StockStatus.Shipped && x.INV_StockStatus.StockStatus != StockStatus.OnOrder,
                    IsSerialized = x.PO_Line.IsSerialized
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

        public async Task UpdateInventoryLocationPutawayAsync(InventoryLocationUpdateDTO inventoryLocationUpdateDTO)
        {
            // Get InStock Stock Status
            var inStockStatus = await DB.INV_StockStatus
                .FirstOrDefaultAsync(x => x.StockStatus == StockStatus.InStock)
                ?? throw new ValidationException("In Stock status not found.");

            // Get Inventory
            var inventory = await DB.INV_Stock
                .Include(x => x.UPL_Location)
                .Include(x => x.PO_Line)
                    .ThenInclude(x => x.UPL_PartInfo)
                        .ThenInclude(x => x.UPL_UOM)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.FormID == inventoryLocationUpdateDTO.FormID)
                ?? throw new ValidationException("Inventory not found.");

            // Check if the location is valid
            var location = await DB.UPL_Location
                .Include(x => x.UPL_UOM)
                .FirstOrDefaultAsync(x => x.LocationNumber == inventoryLocationUpdateDTO.LocationNumber && x.LocationNumber != Locations.Dock)
                ?? throw new ValidationException("Location not found.");

            // Check dimensions
            var boxDimensions = (inventory.PO_Line.UPL_PartInfo.Height ?? 0) * (inventory.PO_Line.UPL_PartInfo.Width ?? 0) * (inventory.PO_Line.UPL_PartInfo.Depth ?? 0);
            var locationDimensions = location.Height * location.Width * location.Depth;

            // Convert to inches
            if (inventory.PO_Line.UPL_PartInfo.UPL_UOM.UnitOfMeasurement == UOM.Feet)
                boxDimensions = boxDimensions * 12 * 12 * 12;

            if (inventory.UPL_Location.UPL_UOM.UnitOfMeasurement == UOM.Feet)
                locationDimensions = locationDimensions * 12 * 12 * 12;

            // Get BoxCount
            var boxCount = await DB.INV_Stock
                    .Where(x => x.LocationID == location.Id && x.INV_StockStatus.StockStatus != StockStatus.Shipped)
                    .SumAsync(x => x.Quantity);

            var boxCountDimensions = boxCount * boxDimensions;

            locationDimensions -= boxCountDimensions;

            // Check if the box fits in the location
            if (boxDimensions > locationDimensions)
                throw new ValidationException($"Box does not fit in the location. {(boxDimensions / 12)} feet needed {(locationDimensions / 12)} available.");

            // Update Inventory
            inventory.UPL_Location = location;
            inventory.INV_StockStatus = inStockStatus;

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task UpdateInventoryLookupAsync(InventoryUpdateLookupDTO inventoryUpdateLookupDTO)
        {
            // Build INV Query
            var invQuery = DB.INV_Stock
                .Include(x => x.UPL_StockGroup)
                .Include(x => x.PO_Line)
                .Where(x => x.FormID == inventoryUpdateLookupDTO.FormID
                    && x.INV_StockStatus.StockStatus != StockStatus.Shipped && x.INV_StockStatus.StockStatus != StockStatus.OnOrder);

            // Get Inventory to Update
            var inventory = await invQuery
                .FirstOrDefaultAsync()
                ?? throw new ValidationException("Inventory not found.");

            // Check Serial
            if (inventory.PO_Line.IsSerialized && string.IsNullOrEmpty(inventoryUpdateLookupDTO.SerialNumber))
                throw new ValidationException("Serial Number is required.");

            // Update Location
            await UpdateInventoryLocationPutawayAsync(new InventoryLocationUpdateDTO
            {
                FormID = inventoryUpdateLookupDTO.FormID,
                LocationNumber = inventoryUpdateLookupDTO.LocationNumber
            });

            // Reload 
            inventory = await invQuery
                .FirstOrDefaultAsync()
                ?? throw new ValidationException("Inventory not found.");

            // Get StockGroup
            var stockGroup = await DB.UPL_StockGroup
                .FirstOrDefaultAsync(x => x.FormID == inventoryUpdateLookupDTO.StockGroupID)
                ?? throw new ValidationException("Stock Group not found.");

            var stockStatus = await DB.INV_StockStatus
                .FirstOrDefaultAsync(x => x.FormID == inventoryUpdateLookupDTO.StatusID)
                ?? throw new ValidationException("Stock Status not found.");

            // Update Inventory
            inventory.UPL_StockGroup = stockGroup;
            inventory.INV_StockStatus = stockStatus;
            inventory.SerialNumber = inventoryUpdateLookupDTO.SerialNumber;
            inventory.AssetTag = inventoryUpdateLookupDTO.AssetTag;

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task UpdateInventoryLookupUDFAsync(InventoryUpdateLookupUDFDTO inventoryUpdateLookupUDFDTO)
        {
            // Build INV Query
            var inv = await DB.INV_Stock
                .Include(x => x.UPL_StockGroup)
                .Include(x => x.PO_Line)
                .FirstOrDefaultAsync(x => x.FormID == inventoryUpdateLookupUDFDTO.FormID
                    && x.INV_StockStatus.StockStatus != StockStatus.Shipped && x.INV_StockStatus.StockStatus != StockStatus.OnOrder)
                ?? throw new ValidationException("Inventory not found.");

            // Update UDF
            inv.UDF1 = inventoryUpdateLookupUDFDTO.UDF1;
            inv.UDF2 = inventoryUpdateLookupUDFDTO.UDF2;
            inv.UDF3 = inventoryUpdateLookupUDFDTO.UDF3;
            inv.UDF4 = inventoryUpdateLookupUDFDTO.UDF4;
            inv.UDF5 = inventoryUpdateLookupUDFDTO.UDF5;
            inv.UDF6 = inventoryUpdateLookupUDFDTO.UDF6;
            inv.UDF7 = inventoryUpdateLookupUDFDTO.UDF7;
            inv.UDF8 = inventoryUpdateLookupUDFDTO.UDF8;
            inv.UDF9 = inventoryUpdateLookupUDFDTO.UDF9;
            inv.UDF10 = inventoryUpdateLookupUDFDTO.UDF10;

            // Save changes to the database
            await DB.SaveChangesAsync();
        }
    }
}
