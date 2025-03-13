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
                .Include(x => x.PO_Line)
                    .ThenInclude(x => x.UPL_PartInfo)
                .Where(x => x.FormID == partID)
                .AsNoTracking()
                .Select(x => new PoDTO
                {
                    FormID = x.FormID,
                    ProjectID = x.UPL_Project.FormID,
                    PONumber = x.PONumber,
                    POLines = x.PO_Line.Select(i => new POLineDTO
                    {
                        FormID = i.FormID,
                        PartNumber = i.UPL_PartInfo.PartNumber,
                        PartDescription = i.UPL_PartInfo.PartDescription,
                        Quantity = i.Quantity,
                        RecQuantity = DB.INV_Stock.Where(s => s.PO_Line.FormID == i.FormID).Sum(s => s.Quantity)
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
                         x.UPL_Project.ProjectName.Contains(filter)
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
                        ? query.OrderBy(x => x.UPL_Project.ProjectName)
                        : query.OrderByDescending(x => x.UPL_Project.ProjectName);
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
                    ProjectName = x.UPL_Project.ProjectName,
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
    }
}
