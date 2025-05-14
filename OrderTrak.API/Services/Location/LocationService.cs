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

            // Check if UOM is valid
            var UOM = await DB.UPL_UOM
                .FirstOrDefaultAsync(x => x.FormID == locationCreateDTO.UOMID)
                ?? throw new ValidationException("Unit of Measurement not found.");

            // Create new location
            var newLocation = new UPL_Location
            {
                UPL_UOM = UOM,
                LocationNumber = locationCreateDTO.LocationNumber ?? throw new ValidationException("Location Number is required"),
                Height = locationCreateDTO.Height,
                Width = locationCreateDTO.Width,
                Depth = locationCreateDTO.Depth,
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
                 .AsNoTracking()
                .Select(x => new LocationDTO
                {
                    UOMID = x.UPL_UOM.FormID,
                    FormID = x.FormID,
                    LocationNumber = x.LocationNumber,
                    Height = x.Height,
                    Width = x.Width,
                    Depth = x.Depth,
                })
                .FirstOrDefaultAsync()
                ?? throw new ValidationException("Location not found.");
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
                                             x.UPL_UOM.UnitOfMeasurement.Contains(filter)
                                            );
                }
            }

            // Apply Order By
            query = searchQuery.SortColumn switch
            {
                1 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.LocationNumber)
                                        : query.OrderByDescending(x => x.LocationNumber),
                2 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.Height * x.Width * x.Depth)
                                        : query.OrderByDescending(x => x.Height * x.Width * x.Depth),
                3 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.UPL_UOM.UnitOfMeasurement)
                                        : query.OrderByDescending(x => x.UPL_UOM.UnitOfMeasurement),
                _ => query.OrderBy(x => x.Id),
            };
            var locationList = await query
                .Skip(searchQuery.RecordSize * (searchQuery.Page - 1))
                .Take(searchQuery.RecordSize)
                .AsNoTracking()
                .Select(x => new LocationSearchReturnDTO
                {
                    FormID = x.FormID,
                    LocationNumber = x.LocationNumber,
                    Volume = x.Height * x.Width * x.Depth,
                    UnitOfMeasure = x.UPL_UOM.UnitOfMeasurement
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

            // Check if UOM is valid
            var UOM = await DB.UPL_UOM
                .FirstOrDefaultAsync(x => x.FormID == locationUpdateDTO.UOMID)
                ?? throw new ValidationException("Unit of Measurement not found.");

            // Update Location
            location.UPL_UOM = UOM;
            location.LocationNumber = locationUpdateDTO.LocationNumber ?? throw new ValidationException("Location Number is required");
            location.Height = locationUpdateDTO.Height ?? throw new ValidationException("Height is required");
            location.Width = locationUpdateDTO.Width ?? throw new ValidationException("Width is required");
            location.Depth = locationUpdateDTO.Depth ?? throw new ValidationException("Depth is required");

            // Save
            await DB.SaveChangesAsync();
        }
    }
}
