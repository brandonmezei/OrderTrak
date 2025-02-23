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
        private readonly OrderTrakContext _orderTrakContext;
        private readonly IConfiguration _configuration;
        private readonly string _jwtKey;

        public AuthService(OrderTrakContext db, IConfiguration configuration)
        {
            _orderTrakContext = db;
            _configuration = configuration;

            _jwtKey = _configuration["Jwt:Key"] ?? throw new Exception("JWT Key not found");
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
            if (await _orderTrakContext.SYS_Users.AnyAsync(x => x.Email == registerDTO.Email && !x.IsDelete))
                throw new ValidationException("User already exists");

            // Password Check
            PasswordCheck(registerDTO.Password);

            // Hash the password using bcrypt
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);

            // Create new user
            var user = new SYS_User
            {
                UserName = registerDTO.Email.Split('@')[0],
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                Email = registerDTO.Email,
                Password = hashedPassword,
                CreateName = "System"
            };

            // Add the new user to the database
            _orderTrakContext.SYS_Users.Add(user);
            await _orderTrakContext.SaveChangesAsync();
        }

        public async Task<AuthReturnDTO> LoginAsync(LoginDTO loginDTO)
        {
            // Get User From DB
            var user = await _orderTrakContext.SYS_Users
                .FirstOrDefaultAsync(x => x.Email == loginDTO.Email && x.Approved)
                ?? throw new ValidationException(Messages.CannotLogin);

            // Check Password
            if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))
                throw new ValidationException(Messages.CannotLogin);

            // Generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                ]),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new AuthReturnDTO
            {
                Token = tokenString,
                Expiration = tokenDescriptor.Expires.Value,
                UserName = user.UserName,
                Email = user.Email
            };
        }
    }
}
