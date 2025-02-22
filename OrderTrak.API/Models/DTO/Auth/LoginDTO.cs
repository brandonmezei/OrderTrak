using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Auth
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;
    }
}
