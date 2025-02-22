using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO.Auth;
using OrderTrak.API.Models.OrderTrakDB;

namespace OrderTrak.API.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly OrderTrakContext _db;

        public AuthService(OrderTrakContext db)
        {
            _db = db;
        }

        private void PasswordCheck(string password)
        {
            // Apply Basic Password Rules
            if (password.Length < 8)
                throw new Exception("Password must be at least 8 characters long");

            if (!password.Any(char.IsUpper))
                throw new Exception("Password must contain at least one uppercase letter");

            if (!password.Any(char.IsLower))
                throw new Exception("Password must contain at least one lowercase letter");

            if (!password.Any(char.IsDigit))
                throw new Exception("Password must contain at least one digit");

            // Special Character Check
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                throw new Exception("Password must contain at least one special character");

        }

        public async Task Register(RegisterDTO registerDTO)
        {
            // Check if user already exists
            var user = await _db.SYS_Users
                .FirstOrDefaultAsync(x => x.Email == registerDTO.Email && !x.IsDelete);

            if (user != null)
                throw new Exception("User already exists");

            // Password Check
            PasswordCheck(registerDTO.Password);

            // Hash the password using bcrypt
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);

            // Create new user
            user = new SYS_User
            {
                UserName = registerDTO.Email.Split('@')[0],
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                Email = registerDTO.Email,
                Password = hashedPassword,
                CreateName = "System"
            };

            // Add the new user to the database
            _db.SYS_Users.Add(user);
            await _db.SaveChangesAsync();
        }
    }
}
