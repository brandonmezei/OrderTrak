using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Profile
{
    public class ProfileUpdateDTO
    {
        [Required(ErrorMessage = "First Name is required.")]
        [MaxLength(50)]
        public string? FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required.")]
        [MaxLength(50)]
        public string? LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress]
        public string? Email { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? CurrentPassword { get; set; }

        [MaxLength(100)]
        public string? Password { get; set; }
    }
}
