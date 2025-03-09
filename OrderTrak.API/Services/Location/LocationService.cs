using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Location;
using OrderTrak.API.Models.OrderTrakDB;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Services.Location
{
    public class LocationService(OrderTrakContext orderTrakContext) : ILocationService
    {
        private readonly OrderTrakContext DB = orderTrakContext;

        public async Task<Guid> CreateLocationAsync(LocationCreateDTO locationCreateDTO)
        {
            // Check if location already exists
            if (await DB.UPL_Location.AnyAsync(x => x.LocationNumber == locationCreateDTO.LocationNumber))
                throw new ValidationException("Location already exists");

            // Check if Heigh, Width, and Depth are greater than 0
            if (locationCreateDTO.Height <= 0 || locationCreateDTO.Width <= 0 || locationCreateDTO.Depth <= 0)
                throw new ValidationException("Height, Width, and Depth must be greater than 0");

            // Create new location
            var newLocation = new UPL_Location
            {
                LocationNumber = locationCreateDTO.LocationNumber ?? throw new ValidationException("Location Number is required"),
                Height = locationCreateDTO.Height,
                Width = locationCreateDTO.Width,
                Depth = locationCreateDTO.Depth,
                UnitOfMeasure = locationCreateDTO.UnitOfMeasure ?? throw new ValidationException("Unit of Measurement is required")
            };

            // Save
            await DB.UPL_Location.AddAsync(newLocation);
            await DB.SaveChangesAsync();

            return newLocation.FormID;
        }

        public async Task DeleteLocationAsync(Guid locationID)
        {
            // Get Location By ID
            var location = await DB.UPL_Location
                .FirstOrDefaultAsync(x => x.FormID == locationID)
                ?? throw new ValidationException("Location not found");

            // Soft Delete
            location.IsDelete = true;

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task<LocationDTO> GetLocationAsync(Guid locationID)
        {
            return await DB.UPL_Location
                .Where(x => x.FormID == locationID)
                .Select(x => new LocationDTO
                {
                    FormID = x.FormID,
                    LocationNumber = x.LocationNumber,
                    Height = x.Height,
                    Width = x.Width,
                    Depth = x.Depth,
                    UnitOfMeasure = x.UnitOfMeasure
                })
                .FirstOrDefaultAsync()
                ?? throw new ValidationException("Location not found");
        }

        public async Task<PagedTable<LocationSearchReturnDTO>> SearchLocationAsync(SearchQueryDTO searchQuery)
        {
            // Get Location Query
            var query = DB.UPL_Location
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
                    query = query.Where(x => x.LocationNumber.Contains(filter) ||
                                             x.UnitOfMeasure.Contains(filter)
                                            );
                }
            }

            // Apply Order By
            switch (searchQuery.SortColumn)
            {
                case 1:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.LocationNumber)
                        : query.OrderByDescending(x => x.LocationNumber);
                    break;
                case 2:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.Height * x.Width * x.Depth)
                        : query.OrderByDescending(x => x.Height * x.Width * x.Depth);
                    break;
                case 3:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.UnitOfMeasure)
                        : query.OrderByDescending(x => x.UnitOfMeasure);
                    break;
                default:
                    query = query.OrderBy(x => x.Id);
                    break;
            }

            var locationList = await query
                .Skip(searchQuery.RecordSize * (searchQuery.Page - 1))
                .Take(searchQuery.RecordSize)
                .AsNoTracking()
                .Select(x => new LocationSearchReturnDTO
                {
                    FormID = x.FormID,
                    LocationNumber = x.LocationNumber,
                    Volume = x.Height * x.Width * x.Depth,
                    UnitOfMeasure = x.UnitOfMeasure
                })
                .ToListAsync();

            // Return Object
            return new PagedTable<LocationSearchReturnDTO>
            {
                Data = locationList,
                TotalRecords = await query.CountAsync(),
                PageIndex = searchQuery.Page
            };
        }

        public async Task UpdateLocationAsync(LocationUpdateDTO locationUpdateDTO)
        {
            // Get Location
            var location = await DB.UPL_Location
                .FirstOrDefaultAsync(x => x.FormID == locationUpdateDTO.FormID)
                ?? throw new ValidationException("Location not found");

            // Check if location already exists
            if (await DB.UPL_Location.AnyAsync(x => x.LocationNumber == locationUpdateDTO.LocationNumber && x.FormID != locationUpdateDTO.FormID))
                throw new ValidationException("Location already exists");

            // Check if Heigh, Width, and Depth are greater than 0
            if (locationUpdateDTO.Height <= 0 || locationUpdateDTO.Width <= 0 || locationUpdateDTO.Depth <= 0)
                throw new ValidationException("Height, Width, and Depth must be greater than 0");

            // Update Location
            location.LocationNumber = locationUpdateDTO.LocationNumber ?? throw new ValidationException("Location Number is required");
            location.Height = locationUpdateDTO.Height ?? throw new ValidationException("Height is required");
            location.Width = locationUpdateDTO.Width ?? throw new ValidationException("Width is required");
            location.Depth = locationUpdateDTO.Depth ?? throw new ValidationException("Depth is required");
            location.UnitOfMeasure = locationUpdateDTO.UnitOfMeasure ?? throw new ValidationException("Unit of Measurement is required");

            // Save
            await DB.SaveChangesAsync();
        }
    }
}
