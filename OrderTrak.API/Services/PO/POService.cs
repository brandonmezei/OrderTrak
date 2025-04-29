using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.PO;
using OrderTrak.API.Models.OrderTrakDB;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Services.PO
{
    public class POService(OrderTrakContext orderTrakContext) : IPOService
    {
        private readonly OrderTrakContext DB = orderTrakContext;

        public async Task<Guid> CreatePOAsync(POCreateDTO pOCreateDTO)
        {
            // Get Project
            var project = await DB.UPL_Project.FirstOrDefaultAsync(x => x.FormID == pOCreateDTO.ProjectID)
                ?? throw new ValidationException("Project not found.");

            // Check if PO already Exists
            if (await DB.PO_Header.AnyAsync(x => x.PONumber == pOCreateDTO.PONumber && x.UPL_Project.FormID == project.FormID))
                throw new ValidationException("PO already exists in project.");

            // Create New PO
            var newPO = new PO_Header
            {
                UPL_Project = project,
                PONumber = pOCreateDTO.PONumber
            };

            // Save
            await DB.PO_Header.AddAsync(newPO);
            await DB.SaveChangesAsync();

            return newPO.FormID;
        }

        public async Task DeletePOAsync(Guid partID)
        {
            // Get PO
            var po = await DB.PO_Header.FirstOrDefaultAsync(x => x.FormID == partID)
                ?? throw new ValidationException("PO not found.");

            // Check if Po has any Parts
            if (await DB.INV_Stock.AnyAsync(x => x.PO_Line.PO_Header.FormID == po.FormID))
                throw new ValidationException("PO has parts received to it.");

            // Soft Delete PO
            po.IsDelete = true;

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task<PoDTO> GetPOAsync(Guid partID)
        {
            return await DB.PO_Header
                .Include(x => x.UPL_Project)
                    .ThenInclude(x => x.UPL_Customer)
                .Include(x => x.PO_Line)
                    .ThenInclude(x => x.UPL_PartInfo)
                .Where(x => x.FormID == partID)
                .AsNoTracking()
                .Select(x => new PoDTO
                {
                    FormID = x.FormID,
                    ProjectID = x.UPL_Project.FormID,
                    PONumber = x.PONumber,
                    CustomerCode = x.UPL_Project.UPL_Customer.CustomerCode,
                    ProjectCode = x.UPL_Project.ProjectCode,
                    POLines = x.PO_Line.Select(i => new POLineDTO
                    {
                        FormID = i.FormID,
                        PartNumber = i.UPL_PartInfo.PartNumber,
                        PartDescription = i.UPL_PartInfo.PartDescription,
                        Quantity = i.Quantity,
                        RecQuantity = DB.INV_Stock.Where(s => s.PO_Line.FormID == i.FormID).Sum(s => s.Quantity),
                        IsSerialized = i.IsSerialized
                    })
                    .ToList()
                })
                .FirstOrDefaultAsync()
                ?? throw new ValidationException("PO not found.");

        }

        public async Task<PagedTable<POSearchReturnDTO>> SearchPOAsync(POSearchDTO searchQuery)
        {
            var query = DB.PO_Header
               .Include(x => x.UPL_Project)
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
                        x.PONumber.Contains(filter) ||
                         x.UPL_Project.ProjectCode.Contains(filter)
                    );
                }
            }

            if (searchQuery.NoReceipt)
                query = query.Where(x => !x.PO_Line.All(i => i.INV_Stock.Count == 0));

            // Apply Order By
            switch (searchQuery.SortColumn)
            {
                case 1:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.PONumber)
                        : query.OrderByDescending(x => x.PONumber);
                    break;
                case 2:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.UPL_Project.ProjectCode)
                        : query.OrderByDescending(x => x.UPL_Project.ProjectCode);
                    break;
                default:
                    query = query.OrderBy(x => x.Id);
                    break;
            }

            // Apply pagination and projection
            var POList = await query
                .Skip(searchQuery.RecordSize * (searchQuery.Page - 1))
                .Take(searchQuery.RecordSize)
                .AsNoTracking()
                .Select(x => new POSearchReturnDTO
                {
                    FormID = x.FormID,
                    PONumber = x.PONumber,
                    ProjectCode = x.UPL_Project.ProjectCode,
                })
                .ToListAsync();

            // Return Object
            return new PagedTable<POSearchReturnDTO>
            {
                Data = POList,
                TotalRecords = await query.CountAsync(),
                PageIndex = searchQuery.Page
            };
        }

        public async Task UpdatePOAsync(POUpdateDTO pOUpdateDTO)
        {
            // Get PO
            var po = await DB.PO_Header
                .Include(x => x.UPL_Project)
                .FirstOrDefaultAsync(x => x.FormID == pOUpdateDTO.FormID)
                ?? throw new ValidationException("PO not found.");

            // Check if PO already Exists
            if (await DB.PO_Header.AnyAsync(x => x.PONumber == pOUpdateDTO.PONumber && x.UPL_Project.FormID == po.UPL_Project.FormID && x.FormID != pOUpdateDTO.FormID))
                throw new ValidationException("PO already exists in project.");

            // Update PO
            po.PONumber = pOUpdateDTO.PONumber ?? throw new ValidationException("PO Number is required.");

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task CreatePOLineAsync(POCreateLineDTO poLineCreateDTO)
        {
            // Get PO
            var po = await DB.PO_Header
                .Include(x => x.UPL_Project)
                .FirstOrDefaultAsync(x => x.FormID == poLineCreateDTO.OrderID)
                ?? throw new ValidationException("PO not found.");

            // Get Part
            var part = await DB.UPL_PartInfo
                .FirstOrDefaultAsync(x => x.FormID == poLineCreateDTO.PartID)
                ?? throw new ValidationException("Part not found.");

            // Check if Part already exists in PO
            if (await DB.PO_Line.AnyAsync(x => x.UPL_PartInfo.FormID == part.FormID && x.PO_Header.FormID == po.FormID))
                throw new ValidationException("Part already exists on PO.");

            // Make sure the part add doesn't exceed 50
            if (await DB.PO_Line.CountAsync(x => x.PO_Header.FormID == po.FormID) >= 50)
                throw new ValidationException("PO can't have more than 50 parts.");

            // Save
            await DB.PO_Line.AddAsync(new PO_Line
            {
                PO_Header = po,
                UPL_PartInfo = part,
                Quantity = 1
            });


            await DB.SaveChangesAsync();
        }

        public async Task DeletePOLineAsync(Guid FormID)
        {
            // Get PO Line
            var poLine = await DB.PO_Line
                .FirstOrDefaultAsync(x => x.FormID == FormID)
                ?? throw new ValidationException("PO Line not found.");

            // Check if PO Line has any Stock
            if (await DB.INV_Stock.AnyAsync(x => x.PO_Line.FormID == poLine.FormID))
                throw new ValidationException("PO Line has parts received to it.");

            // Soft Delete PO Line
            poLine.IsDelete = true;

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task UpdatePOLineAsync(POUpdateLineDTO poLineUpdateDTO)
        {
            // Get PO Line
            var poLine = await DB.PO_Line
                .FirstOrDefaultAsync(x => x.FormID == poLineUpdateDTO.FormID)
                ?? throw new ValidationException("PO Line not found.");

            // Make sure QTY is greater than 0
            if (poLineUpdateDTO.Quantity <= 0)
                throw new ValidationException("Quantity must be greater than 0.");

            // Make sure QTY isn't less than what is received
            if (poLineUpdateDTO.Quantity < DB.INV_Stock.Where(x => x.PO_Line.FormID == poLine.FormID).Sum(x => x.Quantity))
                throw new ValidationException("Quantity can't be less than what is received.");

            // Update PO Line
            poLine.Quantity = poLineUpdateDTO.Quantity ?? throw new ValidationException("Quantity is required.");
            poLine.IsSerialized = poLineUpdateDTO.IsSerialized;

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task<PagedTable<POLineSearchReturnDTO>> SearchPOLineAsync(SearchQueryDTO searchQuery)
        {
            var query = DB.PO_Line
              .Include(x => x.PO_Header.UPL_Project)
              .Include(x => x.UPL_PartInfo)
              .Include(x => x.INV_Stock)
              .Where(x => x.Quantity > x.INV_Stock.Sum(i => i.Quantity));

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
                        x.UPL_PartInfo.PartNumber.Contains(filter) ||
                         x.PO_Header.PONumber.Contains(filter) ||
                         x.PO_Header.UPL_Project.ProjectCode.Contains(filter)
                    );
                }
            }

            // Apply Order By
            switch (searchQuery.SortColumn)
            {
                case 1:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.PO_Header.PONumber)
                        : query.OrderByDescending(x => x.PO_Header.PONumber);
                    break;
                case 2:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.PO_Header.UPL_Project.ProjectCode)
                        : query.OrderByDescending(x => x.PO_Header.UPL_Project.ProjectCode);
                    break;
                case 3:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.UPL_PartInfo.PartNumber)
                        : query.OrderByDescending(x => x.UPL_PartInfo.PartNumber);
                    break;
                case 4:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.Quantity)
                        : query.OrderByDescending(x => x.Quantity);
                    break;
                case 5:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.INV_Stock.Sum(i => i.Quantity))
                        : query.OrderByDescending(x => x.INV_Stock.Sum(i => i.Quantity));
                    break;
                case 6:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.IsSerialized)
                        : query.OrderByDescending(x => x.IsSerialized);
                    break;
                default:
                    query = query.OrderBy(x => x.Id);
                    break;
            }

            // Apply pagination and projection
            var POPartList = await query
                .Skip(searchQuery.RecordSize * (searchQuery.Page - 1))
                .Take(searchQuery.RecordSize)
                .AsNoTracking()
                .Select(x => new POLineSearchReturnDTO
                {
                    FormID = x.FormID,
                    PONumber = x.PO_Header.PONumber,
                    ProjectCode = x.PO_Header.UPL_Project.ProjectCode,
                    PartNumber = x.UPL_PartInfo.PartNumber,
                    Quantity = x.Quantity,
                    RecQuantity = x.INV_Stock.Sum(i => i.Quantity),
                    IsSerialized = x.IsSerialized,
                    UDF1 = x.PO_Header.UPL_Project.UDF1,
                    UDF2 = x.PO_Header.UPL_Project.UDF2,
                    UDF3 = x.PO_Header.UPL_Project.UDF3,
                    UDF4 = x.PO_Header.UPL_Project.UDF4,
                    UDF5 = x.PO_Header.UPL_Project.UDF5,
                    UDF6 = x.PO_Header.UPL_Project.UDF6,
                    UDF7 = x.PO_Header.UPL_Project.UDF7,
                    UDF8 = x.PO_Header.UPL_Project.UDF8,
                    UDF9 = x.PO_Header.UPL_Project.UDF9,
                    UDF10 = x.PO_Header.UPL_Project.UDF10
                })
                .ToListAsync();

            // Return Object
            return new PagedTable<POLineSearchReturnDTO>
            {
                Data = POPartList,
                TotalRecords = await query.CountAsync(),
                PageIndex = searchQuery.Page
            };
        }
    }
}
