﻿using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Parts;

namespace OrderTrak.API.Services.Parts
{
    public interface IPartService
    {
        Task<Guid> CreatePartAsync(PartCreateDTO partCreateDTO);
        Task UpdatePartAsync(PartUpdateDTO partUpdateDTO);
        Task DeletePartAsync(Guid partID);
        Task<PartDTO> GetPartAsync(Guid partID);
        Task<PagedTable<PartSearchReturnDTO>> SearchPartsAsync(PartSearchDTO searchQuery);
    }
}
