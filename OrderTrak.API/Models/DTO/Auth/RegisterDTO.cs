using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Auth
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "First Name is required.")]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [MaxLength(50)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(100)]
        public string? Password { get; set; }
    }
}
