using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Customer
{
    public class CustomerUpdateDTO
    {
        [Required]
        public Guid? FormID { get; set; }

        [Required(ErrorMessage = "Customer Code is required.")]
        [MaxLength(4)]
        public string? CustomerCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Customer Name is required.")]
        public string? CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; } = string.Empty;

        public string? Address2 { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string? City { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required.")]
        public string? State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Zip is required.")]
        public string? Zip { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required.")]
        public string? Phone { get; set; } = string.Empty;
    }
}
