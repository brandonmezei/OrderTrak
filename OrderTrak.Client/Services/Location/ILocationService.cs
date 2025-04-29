using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Location
{
    public interface ILocationService
    {
        Task<Guid> CreateLocationAsync(LocationCreateDTO locationCreateDTO);
        Task UpdateLocationAsync(LocationUpdateDTO locationUpdateDTO);
        Task DeleteLocationAsync(Guid locationID);
        Task<LocationDTO> GetLocationAsync(Guid locationID);
        Task<PagedTableOfLocationSearchReturnDTO> SearchLocationAsync(SearchQueryDTO searchQuery);
    }
}
