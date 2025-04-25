using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Location;
using OrderTrak.API.Models.DTO.Receiving;
using OrderTrak.API.Models.OrderTrakDB;
using OrderTrak.API.Services.Location;
using OrderTrak.API.Static;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Services.Receiving
{
    public class ReceivingService(OrderTrakContext orderTrakContext, ILocationService injectedLocationService) : IReceivingService
    {

        private readonly OrderTrakContext DB = orderTrakContext;
        private readonly ILocationService locationService = injectedLocationService;

        public async Task<Guid> CreateReceivingAsync(ReceivingCreateDTO receivingCreateDTO)
        {
            // Add the new receiving record to the database
            var newReceiving = new INV_Receipt
            {
                TrackingNumber = receivingCreateDTO.TrackingNumber,
                Carrier = receivingCreateDTO.Carrier
            };

            // Save
            DB.INV_Receipt.Add(newReceiving);
            await DB.SaveChangesAsync();

            return newReceiving.FormID;
        }

        public async Task DeleteReceivingAsync(Guid recID)
        {
            // Find the Record
            var rec = DB.INV_Receipt
                 .FirstOrDefault(x => x.FormID == recID)
                 ?? throw new ValidationException("Receiving record not found.");

            // Check to make sure it is empty
            if (await DB.INV_Stock.AnyAsync(x => x.ReceiptID == rec.Id))
                throw new ValidationException("Receiving record is not empty.");

            // Check to make sure the receipt is on today's date
            if (rec.CreateDate.Date != DateTime.Today.Date)
                throw new ValidationException("Receiving record is not on today's date.");

            // Soft Delete Record
            rec.IsDelete = true;

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task<ReceivingDTO> GetReceivingAsync(Guid recID)
        {
            // Get Receipt
            return await DB.INV_Receipt
                .Include(x => x.INV_Stock)
                    .ThenInclude(x => x.PO_Line)
                        .ThenInclude(x => x.UPL_PartInfo)
                 .Include(x => x.INV_Stock)
                    .ThenInclude(x => x.PO_Line)
                        .ThenInclude(x => x.PO_Header)
                .Where(x => x.FormID == recID)
                .AsNoTracking()
                .Select(x => new ReceivingDTO
                {
                    FormID = x.FormID,
                    TrackingNumber = x.TrackingNumber,
                    Carrier = x.Carrier,
                    DataReceived = x.CreateDate,
                    CanReceive = DateTime.Today.Date == x.CreateDate.Date,
                    ReceivingLines = x.INV_Stock
                        .GroupBy(x => new
                        {
                            x.PO_Line.UPL_PartInfo.PartNumber,
                            x.PO_Line.UPL_PartInfo.PartDescription,
                            x.PO_Line.PO_Header.PONumber
                        })
                        .Select(i => new ReceivingLineDTO
                        {
                            PartNumber = i.Key.PartNumber,
                            PartDescription = i.Key.PartDescription,
                            PurchaseOrder = i.Key.PONumber,
                            Quantity = i.Sum(s => s.Quantity)
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync()
                ?? throw new ValidationException("Receiving record not found");
        }

        public async Task<PagedTable<ReceivingSearchReturnDTO>> SearchReceivingAsync(ReceivingSearchDTO searchQuery)
        {
            // Get Rec
            var query = DB.INV_Receipt
                .Include(x => x.INV_Stock)
                    .ThenInclude(x => x.PO_Line.PO_Header)
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
                    query = query.Where(x => x.TrackingNumber.Contains(filter) ||
                                             x.Carrier.Contains(filter));
                }
            }

            // Empty Filter
            if (searchQuery.IsEmpty)
                query = query.Where(x => !x.INV_Stock.Any());

            // Date Filter
            if (searchQuery.IsToday)
                query = query.Where(x => x.CreateDate.Date == DateTime.Today.Date);

            // Apply Order By
            switch (searchQuery.SortColumn)
            {
                case 1:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.TrackingNumber)
                        : query.OrderByDescending(x => x.TrackingNumber);
                    break;
                case 2:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.Carrier)
                        : query.OrderByDescending(x => x.Carrier);
                    break;
                case 3:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.INV_Stock.Select(i => i.PO_Line.PO_Header.Id).Distinct().Count())
                        : query.OrderByDescending(x => x.INV_Stock.Select(i => i.PO_Line.PO_Header.Id).Distinct().Count());
                    break;
                case 4:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.INV_Stock.Sum(x => x.Quantity))
                        : query.OrderByDescending(x => x.INV_Stock.Sum(x => x.Quantity));
                    break;
                case 5:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.CreateDate)
                        : query.OrderByDescending(x => x.CreateDate);
                    break;
                default:
                    query = query.OrderBy(x => x.Id);
                    break;
            }

            // Apply pagination and projection
            var trackingList = await query
                .Skip(searchQuery.RecordSize * (searchQuery.Page - 1))
                .Take(searchQuery.RecordSize)
                .AsNoTracking()
                .Select(x => new ReceivingSearchReturnDTO
                {
                    FormID = x.FormID,
                    TrackingNumber = x.TrackingNumber,
                    Carrier = x.Carrier,
                    POCount = x.INV_Stock.Select(i => i.PO_Line.PO_Header.Id).Distinct().Count(),
                    QuantityReceived = x.INV_Stock.Sum(x => x.Quantity),
                    DataReceived = x.CreateDate
                })
                .ToListAsync();

            // Return Object
            return new PagedTable<ReceivingSearchReturnDTO>
            {
                Data = trackingList,
                TotalRecords = await query.CountAsync(),
                PageIndex = searchQuery.Page
            };
        }

        public async Task CreateReceivingLineAsync(ReceivingLineCreateDTO receivingLineCreateDTO)
        {

            // Get Receipt by RecID
            var receipt = await DB.INV_Receipt
                .FirstOrDefaultAsync(x => x.FormID == receivingLineCreateDTO.RecID)
                ?? throw new ValidationException("Receiving record not found.");

            // Get PO Line by POLineID
            var poLine = await DB.PO_Line
                .FirstOrDefaultAsync(x => x.FormID == receivingLineCreateDTO.PoLineID)
                ?? throw new ValidationException("PO Line not found.");

            // Get Stock Group by StockGroupID
            var stockGroup = await DB.UPL_StockGroup
                .FirstOrDefaultAsync(x => x.FormID == receivingLineCreateDTO.StockGroupID)
                ?? throw new ValidationException("Stock Group not found.");

            // Get Received Status
            var stockStatus = await DB.INV_StockStatus
                .FirstOrDefaultAsync(x => x.StockStatus == StockStatus.Received)
                ?? throw new ValidationException("Stock Status Received not found.");

            // Get Dock Location
            var dockLocation = await DB.UPL_Location
                .FirstOrDefaultAsync(x => x.LocationNumber == Locations.Dock);

            // Create Dock if it doesn't exist
            if (dockLocation == null)
            {
                // Get Feet UOM
                var feetUOM = await DB.UPL_UOM
                    .FirstOrDefaultAsync(x => x.UnitOfMeasurement == UOM.Feet)
                    ?? throw new ValidationException("Feet UOM not found.");

                await locationService.CreateLocationAsync(new LocationCreateDTO
                {
                    LocationNumber = Locations.Dock,
                    Height = 1,
                    Width = 1,
                    Depth = 1,
                    UOMID = feetUOM.FormID
                });

                // Get Dock Location
                dockLocation = await DB.UPL_Location
                    .FirstOrDefaultAsync(x => x.LocationNumber == Locations.Dock)
                    ?? throw new ValidationException("Dock Location not found.");
            }

            // Check if total receipt qty plus what is already on stock for the PO line will exceed the po line qty
            var totalQty = await DB.INV_Stock
                .Where(x => x.PO_Line.FormID == receivingLineCreateDTO.PoLineID)
                .SumAsync(x => x.Quantity);

            if (totalQty + receivingLineCreateDTO.BoxLineList.Sum(x => x.Quantity ?? 0) > poLine.Quantity)
                throw new ValidationException("Total quantity received exceeds PO line quantity.");

            // Check if Serial Numbers are unique in upload if Poline is serialized
            if (poLine.IsSerialized)
            {
                var serialNumbers = receivingLineCreateDTO.BoxLineList
                    .Select(x => x.SerialNumber)
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

                // Check if serial numbers are unique
                if (serialNumbers.Distinct().Count() != serialNumbers.Count)
                    throw new ValidationException("Serial Numbers must be unique.");
            }

            // Loop through each box line
            foreach (var line in receivingLineCreateDTO.BoxLineList)
            {
                // Qty Must be greater than 0
                if (!line.Quantity.HasValue || line.Quantity <= 0)
                    throw new ValidationException("Quantity must be greater than 0.");

                // Create New Receipt
                var newStock = new INV_Stock
                {
                    INV_Receipt = receipt,
                    PO_Line = poLine,
                    INV_StockStatus = stockStatus,
                    UPL_StockGroup = stockGroup,
                    UPL_Location = dockLocation,
                    Quantity = line.Quantity ?? 1
                };

                // Serialized Logic
                if (poLine.IsSerialized)
                {
                    // Set new Stock QTY to 1
                    newStock.Quantity = 1;

                    // Check Serial is Empty
                    if (string.IsNullOrEmpty(line.SerialNumber))
                        throw new ValidationException("Serial Number is required.");

                    // Check Serial is Unique
                    if (await DB.INV_Stock.AnyAsync(x => x.SerialNumber == line.SerialNumber && x.INV_StockStatus.StockStatus != StockStatus.Shipped))
                        throw new ValidationException($"Serial Number {line.SerialNumber} already exists.");

                    // Set Serial Number and Asset Tag
                    newStock.SerialNumber = line.SerialNumber;
                    newStock.AssetTag = line.AssetTag;
                }

                // Reference Logic
                newStock.UDF1 = line.UDF1;
                newStock.UDF2 = line.UDF2;
                newStock.UDF3 = line.UDF3;
                newStock.UDF4 = line.UDF4;
                newStock.UDF5 = line.UDF5;
                newStock.UDF6 = line.UDF6;
                newStock.UDF7 = line.UDF7;
                newStock.UDF8 = line.UDF8;
                newStock.UDF9 = line.UDF9;
                newStock.UDF10 = line.UDF10;

                DB.INV_Stock.Add(newStock);
            }

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task UpdateReceivingAsync(ReceivingUpdateDTO receivingUpdateDTO)
        {
            // Get the Receipt by FormID
            var receipt = DB.INV_Receipt
                .FirstOrDefault(x => x.FormID == receivingUpdateDTO.FormID)
                ?? throw new ValidationException("Receiving record not found.");

            // Check if the receipt is on today's date
            if (receipt.CreateDate.Date != DateTime.Today.Date)
                throw new ValidationException("Receiving record is not on today's date.");

            // Update Details
            receipt.TrackingNumber = receivingUpdateDTO.TrackingNumber ?? throw new ValidationException("Tracking Number cannot be blank.");
            receipt.Carrier = receivingUpdateDTO.Carrier ?? throw new ValidationException("Carrier cannot be blank.");

            // Save
            await DB.SaveChangesAsync();
        }
    }
}
