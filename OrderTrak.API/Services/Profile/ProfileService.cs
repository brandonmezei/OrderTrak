using Microsoft.EntityFrameworkCore;
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
                    Email = x.Email
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
    }
}
