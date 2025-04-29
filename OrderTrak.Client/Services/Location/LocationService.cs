using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Location
{
    public class LocationService(IClient client) : ILocationService
    {
        private readonly IClient ApiClient = client;

        public async Task<Guid> CreateLocationAsync(LocationCreateDTO locationCreateDTO)
        {
            return await ApiClient.CreateLocationAsync(locationCreateDTO);
        }

        public async Task DeleteLocationAsync(Guid locationID)
        {
            await ApiClient.DeleteLocationAsync(locationID);
        }

        public async Task<LocationDTO> GetLocationAsync(Guid locationID)
        {
            return await ApiClient.GetLocationAsync(locationID);
        }

        public async Task<PagedTableOfLocationSearchReturnDTO> SearchLocationAsync(SearchQueryDTO searchQuery)
        {
            return await ApiClient.SearchLocationAsync(searchQuery);
        }

        public async Task UpdateLocationAsync(LocationUpdateDTO locationUpdateDTO)
        {
            await ApiClient.UpdateLocationAsync(locationUpdateDTO);
        }
    }
}
