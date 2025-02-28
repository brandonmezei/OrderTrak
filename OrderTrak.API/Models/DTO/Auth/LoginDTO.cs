using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Auth
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Email Address is not in right format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;
    }
}
