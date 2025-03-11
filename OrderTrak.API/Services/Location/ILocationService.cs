using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Location;

namespace OrderTrak.API.Services.Location
{
    public interface ILocationService
    {
        Task<Guid> CreateLocationAsync(LocationCreateDTO locationCreateDTO);
        Task UpdateLocationAsync(LocationUpdateDTO locationUpdateDTO);
        Task DeleteLocationAsync(Guid locationID);
        Task<LocationDTO> GetLocationAsync(Guid locationID);
        Task<PagedTable<LocationSearchReturnDTO>> SearchLocationAsync(SearchQueryDTO searchQuery);
    }
}
