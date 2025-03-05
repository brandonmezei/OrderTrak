using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Profile
{
    public class UserAdminUpdateDTO
    {
        [Required]
        public Guid? FormID { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [MaxLength(50)]
        public string? FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required.")]
        [MaxLength(50)]
        public string? LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress]
        public string? Email { get; set; } = string.Empty;

        [Required]
        public bool Approved { get; set; }
    }
}
