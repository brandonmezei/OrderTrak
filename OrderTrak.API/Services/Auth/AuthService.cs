using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderTrak.API.Models.DTO.Auth;
using OrderTrak.API.Models.OrderTrakDB;
using OrderTrak.API.Static;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrderTrak.API.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly OrderTrakContext DB;
        private readonly IConfiguration Configuration;
        private readonly string JwtKey;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public AuthService(OrderTrakContext db, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            DB = db;
            Configuration = configuration;
            HttpContextAccessor = httpContextAccessor;

            JwtKey = Configuration["Jwt:Key"] ?? throw new Exception("JWT Key not found");
        }

        private static void PasswordCheck(string password)
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

        public async Task RegisterAsync(RegisterDTO registerDTO)
        {
            // Check if user already exists
            if (await DB.SYS_Users.AnyAsync(x => x.Email == registerDTO.Email && !x.IsDelete))
                throw new ValidationException("User already exists");

            // Password Check
            PasswordCheck(registerDTO.Password ?? throw new ValidationException("Password Required."));

            // Hash the password using bcrypt
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);

            // Create new user
            var user = new SYS_User
            {
                UserName = registerDTO.Email?.Split('@')[0] ?? throw new ValidationException("Email Required."),
                FirstName = registerDTO.FirstName ?? throw new ValidationException("First Name Required."),
                LastName = registerDTO.LastName ?? throw new ValidationException("Last Name Required."),
                Email = registerDTO.Email,
                Password = hashedPassword,
                CreateName = "System"
            };

            // Add the new user to the database
            DB.SYS_Users.Add(user);
            await DB.SaveChangesAsync();
        }

        public async Task<AuthReturnDTO> LoginAsync(LoginDTO loginDTO)
        {
            // Get User From DB
            var user = await DB.SYS_Users
                .FirstOrDefaultAsync(x => x.Email == loginDTO.Email && x.Approved)
                ?? throw new ValidationException(Messages.CannotLogin);

            // Check Password
            if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))
                throw new ValidationException(Messages.CannotLogin);

            // Generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                ]),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = Configuration["Jwt:Issuer"],
                Audience = Configuration["Jwt:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new AuthReturnDTO
            {
                Token = tokenString,
                Expiration = tokenDescriptor.Expires.Value,
                UserName = user.UserName,
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}"
            };
        }

        public async Task<List<string>> FetchPermissionsAsync()
        {
            var username = HttpContextAccessor.HttpContext?.Items["Username"]?.ToString() ?? "System";

            var user = await DB.SYS_Users
                .Include(x => x.SYS_Roles)
                    .ThenInclude(x => x.SYS_RolesToFunction.Where(i => i.CanAccess))
                        .ThenInclude(x => x.SYS_Function)
                .FirstOrDefaultAsync(x => x.UserName == username && x.Approved)
                ?? throw new ValidationException("User not found");

            return [.. user.SYS_Roles.SYS_RolesToFunction.Select(x => x.SYS_Function.FunctionName)];
        }
    }
}
