using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Parts;
using OrderTrak.API.Models.OrderTrakDB;
using OrderTrak.API.Static;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Services.Parts
{
    public class PartService(OrderTrakContext orderTrakContext) : IPartService
    {
        private readonly OrderTrakContext DB = orderTrakContext;

        public async Task<Guid> CreatePartAsync(PartCreateDTO partCreateDTO)
        {
            // Check if part already exists
            if (await DB.UPL_PartInfo.AnyAsync(x => x.PartNumber == partCreateDTO.PartNumber))
                throw new ValidationException("Part already exists");

            // Check if PartCost is greater than 0
            if (partCreateDTO.PartCost <= 0)
                throw new ValidationException("Part Cost must be greater than 0");

            // Check if UOM is valid
            var unit = await DB.UPL_UOM
                .FirstOrDefaultAsync(x => x.FormID == partCreateDTO.UOMID)
                ?? throw new ValidationException("Unit of Measurement not found");

            // Check if UOM is valid for stock type
            if (partCreateDTO.IsStock && (unit.UnitOfMeasurement != UOM.Feet && unit.UnitOfMeasurement != UOM.Inches))
                throw new ValidationException("Unit of Measurement must be Feet or Inches for stock parts");

            if (!partCreateDTO.IsStock && (unit.UnitOfMeasurement != UOM.Hours && unit.UnitOfMeasurement != UOM.Minutes))
                throw new ValidationException("Unit of Measurement must be Hours or Minutes for non-stock parts");

            // Create new part
            var newPart = new UPL_PartInfo
            {
                UPL_UOM = unit,
                PartNumber = partCreateDTO.PartNumber,
                PartDescription = partCreateDTO.PartDescription,
                PartType = partCreateDTO.PartType,
                PartVendor = partCreateDTO.PartVendor,
                PartCost = partCreateDTO.PartCost,
                IsStock = partCreateDTO.IsStock,
                Height = partCreateDTO.Height,
                Width = partCreateDTO.Width,
                Depth = partCreateDTO.Depth
            };

            // Save
            await DB.UPL_PartInfo.AddAsync(newPart);
            await DB.SaveChangesAsync();

            return newPart.FormID;
        }

        public async Task UpdatePartAsync(PartUpdateDTO partUpdateDTO)
        {
            var part = await DB.UPL_PartInfo
                .FirstOrDefaultAsync(x => x.FormID == partUpdateDTO.FormID)
                ?? throw new ValidationException("Part not found");

            // Check if PartCost is greater than 0
            if (partUpdateDTO.PartCost <= 0)
                throw new ValidationException("Part Cost must be greater than 0");

            // Check if UOM is valid
            var unit = await DB.UPL_UOM
                .FirstOrDefaultAsync(x => x.FormID == partUpdateDTO.UOMID)
                ?? throw new ValidationException("Unit of Measurement not found");

            // Check if UOM is valid for stock type
            if (partUpdateDTO.IsStock && (unit.UnitOfMeasurement != UOM.Feet && unit.UnitOfMeasurement != UOM.Inches))
                throw new ValidationException("Unit of Measurement must be Feet or Inches for stock parts");

            if (!partUpdateDTO.IsStock && (unit.UnitOfMeasurement != UOM.Hours && unit.UnitOfMeasurement != UOM.Minutes))
                throw new ValidationException("Unit of Measurement must be Hours or Minutes for non-stock parts");

            // Update Fields
            part.UPL_UOM = unit;
            part.PartNumber = partUpdateDTO.PartNumber ?? throw new ValidationException("Part Number is required.");
            part.PartDescription = partUpdateDTO.PartDescription ?? throw new ValidationException("Part Description is required.");
            part.PartType = partUpdateDTO.PartType ?? throw new ValidationException("Part Type is required.");
            part.PartVendor = partUpdateDTO.PartVendor ?? throw new ValidationException("Part Vendor is required.");
            part.PartCost = partUpdateDTO.PartCost ?? throw new ValidationException("Part Cost is required.");
            part.IsStock = partUpdateDTO.IsStock;
            part.Height = partUpdateDTO.Height;
            part.Width = partUpdateDTO.Width;
            part.Depth = partUpdateDTO.Depth;

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task DeletePartAsync(Guid partID)
        {
            var part = await DB.UPL_PartInfo
                .FirstOrDefaultAsync(x => x.FormID == partID)
                ?? throw new ValidationException("Part not found");

            // Delete
            part.IsDelete = true;

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task<PartDTO> GetPartAsync(Guid partID)
        {
            return await DB.UPL_PartInfo
                .Include(x => x.UPL_UOM)
                .Where(x => x.FormID == partID)
                 .AsNoTracking()
                .Select(x => new PartDTO
                {
                    FormID = x.FormID,
                    UOMID = x.UPL_UOM.FormID,
                    PartNumber = x.PartNumber,
                    PartDescription = x.PartDescription,
                    PartType = x.PartType,
                    PartVendor = x.PartVendor,
                    PartCost = x.PartCost,
                    IsStock = x.IsStock,
                    Height = x.Height,
                    Width = x.Width,
                    Depth = x.Depth
                })
                .FirstOrDefaultAsync()
                ?? throw new ValidationException("Part not found");
        }

        public async Task<PagedTable<PartSearchReturnDTO>> SearchPartsAsync(PartSearchDTO searchQuery)
        {
            // Get Parts
            var query = DB.UPL_PartInfo
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
                    query = query.Where(x => x.PartNumber.Contains(filter) ||
                                             x.PartDescription.Contains(filter) ||
                                             x.PartType.Contains(filter));
                }
            }

            // Stock Only Filter
            if (searchQuery.IsStockOnly)
                query = query.Where(x => x.IsStock);

            // Apply Order By
            switch (searchQuery.SortColumn)
            {
                case 1:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.PartNumber)
                        : query.OrderByDescending(x => x.PartNumber);
                    break;
                case 2:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.PartDescription)
                        : query.OrderByDescending(x => x.PartDescription);
                    break;
                case 3:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.PartType)
                        : query.OrderByDescending(x => x.PartType);
                    break;
                case 4:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.IsStock)
                        : query.OrderByDescending(x => x.IsStock);
                    break;
                default:
                    query = query.OrderBy(x => x.Id);
                    break;
            }

            // Apply pagination and projection
            var customerList = await query
                .Skip(searchQuery.RecordSize * (searchQuery.Page - 1))
                .Take(searchQuery.RecordSize)
                .AsNoTracking()
                .Select(x => new PartSearchReturnDTO
                {
                    FormID = x.FormID,
                    PartNumber = x.PartNumber,
                    PartDescription = x.PartDescription,
                    PartType = x.PartType,
                    IsStock = x.IsStock
                })
                .ToListAsync();

            // Return Object
            return new PagedTable<PartSearchReturnDTO>
            {
                Data = customerList,
                TotalRecords = await query.CountAsync(),
                PageIndex = searchQuery.Page
            };
        }
    }
}
