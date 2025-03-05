using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Profile;
using OrderTrak.API.Models.OrderTrakDB;
using OrderTrak.API.Services.Auth;
using OrderTrak.API.Static;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Services.Profile
{
    public class ProfileService(OrderTrakContext orderTrakContext, IAuthService authService) : IProfileService
    {
        private readonly OrderTrakContext DB = orderTrakContext;
        private readonly IAuthService AuthService = authService;


        public async Task<ProfileDTO> GetUserProfileAsync()
        {
            return await DB.SYS_Users
                .Where(x => x.UserName == DB.GetLoggedInUsername() && !x.IsDelete && x.Approved)
                .Select(x => new ProfileDTO
                {
                    FormID = x.FormID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Approved = x.Approved
                })
                .FirstOrDefaultAsync()
                ?? throw new ValidationException(Messages.UserNotFound);
        }

        public async Task UpdateProfileAsync(ProfileUpdateDTO profileUpdateDTO)
        {
            // Get User
            var user = await DB.SYS_Users
                .FirstOrDefaultAsync(x => x.UserName == DB.GetLoggedInUsername() && !x.IsDelete && x.Approved)
                ?? throw new ValidationException(Messages.UserNotFound);

            // Update User Fields
            user.FirstName = profileUpdateDTO.FirstName ?? throw new ValidationException("First Name is required");
            user.LastName = profileUpdateDTO.LastName ?? throw new ValidationException("Last Name is required");
            user.Email = profileUpdateDTO.Email ?? throw new ValidationException("Email Address is required");

            // Update Password only if present
            if (!string.IsNullOrEmpty(profileUpdateDTO.CurrentPassword) && !string.IsNullOrEmpty(profileUpdateDTO.Password))
            {
                // Check Password
                if (!BCrypt.Net.BCrypt.Verify(profileUpdateDTO.CurrentPassword, user.Password))
                    throw new ValidationException("Current Password is not correct.");

                AuthService.PasswordCheck(profileUpdateDTO.Password);
                user.Password = BCrypt.Net.BCrypt.HashPassword(profileUpdateDTO.Password);
            }

            // Save Changes
            await DB.SaveChangesAsync();
        }

        public async Task<PagedTable<ProfileDTO>> SearchUserProfileAsync(SearchQueryDTO searchQuery)
        {
            var query = DB.SYS_Users
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
                        x.Email.Contains(filter) 
                        || x.FirstName.Contains(filter) 
                        || x.LastName.Contains(filter)
                    );
                }
            }

            // Apply Order By
            switch (searchQuery.SortColumn)
            {
                case 1:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.FirstName)
                        : query.OrderByDescending(x => x.FirstName);
                    break;
                case 2:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.LastName)
                        : query.OrderByDescending(x => x.LastName);
                    break;
                case 3:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.Email)
                        : query.OrderByDescending(x => x.Email);
                    break;
                case 4:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.Approved)
                        : query.OrderByDescending(x => x.Approved);
                    break;
                default:
                    query = query.OrderBy(x => x.Id);
                    break;
            }

            var profileList = await query
               .Skip(searchQuery.RecordSize * (searchQuery.Page - 1))
               .Take(searchQuery.RecordSize)
               .AsNoTracking()
                .Select(x => new ProfileDTO
                {
                    FormID = x.FormID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Approved = x.Approved
                })
                .ToListAsync();

            return new PagedTable<ProfileDTO>
            {
                Data = profileList,
                TotalRecords = await query.CountAsync(),
                PageIndex = searchQuery.Page
            };
        }

        public async Task UpdateUserAdminAsync(UserAdminUpdateDTO userAdminUpdateDTO)
        {
            // Get User
            var user = await DB.SYS_Users
                .FirstOrDefaultAsync(x => x.FormID == userAdminUpdateDTO.FormID && !x.IsDelete)
                ?? throw new ValidationException(Messages.UserNotFound);

            if(user.UserName == DB.GetLoggedInUsername() && !userAdminUpdateDTO.Approved)
                throw new ValidationException("You cannot revoke approval on your own profile.");

            // Update User Fields
            user.FirstName = userAdminUpdateDTO.FirstName ?? throw new ValidationException("First Name is required");
            user.LastName = userAdminUpdateDTO.LastName ?? throw new ValidationException("Last Name is required");
            user.Email = userAdminUpdateDTO.Email ?? throw new ValidationException("Email Address is required");

            // Update Approved
            user.Approved = userAdminUpdateDTO.Approved;

            // Save Changes
            await DB.SaveChangesAsync();
        }

        public async Task<ProfileDTO> GetUserProfileAsync(Guid FormID)
        {
            return await DB.SYS_Users
                .Where(x => x.FormID == FormID && !x.IsDelete)
                .Select(x => new ProfileDTO
                {
                    FormID = x.FormID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Approved = x.Approved
                })
                .FirstOrDefaultAsync()
                ?? throw new ValidationException(Messages.UserNotFound);
        }

        public async Task DeleteUserAdminAsync(Guid FormID)
        {
            // Get User
            var user = await DB.SYS_Users
                .FirstOrDefaultAsync(x => x.FormID == FormID && !x.IsDelete)
                ?? throw new ValidationException(Messages.UserNotFound);

            if (user.UserName == DB.GetLoggedInUsername())
                throw new ValidationException("You cannot delete your own profile.");

            // Soft Delete
            user.IsDelete = true;

            await DB.SaveChangesAsync();
        }
    }
}
