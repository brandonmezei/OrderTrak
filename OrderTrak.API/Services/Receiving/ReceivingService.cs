using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Receiving;
using OrderTrak.API.Models.OrderTrakDB;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Services.Receiving
{
    public class ReceivingService(OrderTrakContext orderTrakContext) : IReceivingService
    {

        private readonly OrderTrakContext DB = orderTrakContext;

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
                        .GroupBy(x => new { 
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
            if(searchQuery.IsEmpty)
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
                        ? query.OrderBy(x => x.INV_Stock.Select(i => i.POLineID).Distinct().Count())
                        : query.OrderByDescending(x => x.INV_Stock.Select(i => i.POLineID).Distinct().Count());
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
                    POCount = x.INV_Stock.Select(i => i.POLineID).Distinct().Count(),
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
    }
}
