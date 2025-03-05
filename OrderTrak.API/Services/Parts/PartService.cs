using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO.Parts;
using OrderTrak.API.Models.OrderTrakDB;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Services.Parts
{
    public class PartService(OrderTrakContext orderTrakContext) : IPartService
    {
        private readonly OrderTrakContext DB = orderTrakContext;

        public async Task<Guid> CreatePartAsync(PartCreateDTO partCreateDTO)
        {
            // Check if part already exists
            if(await DB.UPL_PartInfo.AnyAsync(x => x.PartNumber == partCreateDTO.PartNumber && !x.IsDelete))
                throw new ValidationException("Part already exists");

            // Create new part
            var newPart = new UPL_PartInfo
            {
                PartNumber = partCreateDTO.PartNumber,
                PartDescription = partCreateDTO.PartDescription,
                PartType = partCreateDTO.PartType,
                PartVendor = partCreateDTO.PartVendor,
                PartCost = partCreateDTO.PartCost,
                PartUnit = partCreateDTO.PartUnit,
                IsStock = partCreateDTO.IsStock
            };

            // Save
            await DB.UPL_PartInfo.AddAsync(newPart);
            await DB.SaveChangesAsync();

            return newPart.FormID;
        }

        public async Task UpdatePartAsync(PartUpdateDTO partUpdateDTO)
        {
            var part = await DB.UPL_PartInfo
                .FirstOrDefaultAsync(x => x.FormID == partUpdateDTO.FormID && !x.IsDelete)
                ?? throw new ValidationException("Part not found");

            // Update Fields
            part.PartNumber = partUpdateDTO.PartNumber ?? throw new ValidationException("Part Number is required.");
            part.PartDescription = partUpdateDTO.PartDescription ?? throw new ValidationException("Part Description is required.");
            part.PartType = partUpdateDTO.PartType ?? throw new ValidationException("Part Type is required.");
            part.PartVendor = partUpdateDTO.PartVendor ?? throw new ValidationException("Part Vendor is required.");
            part.PartCost = partUpdateDTO.PartCost ?? throw new ValidationException("Part Cost is required.");
            part.PartUnit = partUpdateDTO.PartUnit ?? throw new ValidationException("Part Unit is required.");
            part.IsStock = partUpdateDTO.IsStock;

            // Save
            await DB.SaveChangesAsync();
        }
    }
}
