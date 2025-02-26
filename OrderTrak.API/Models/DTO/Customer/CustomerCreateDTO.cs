using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Customer
{
    public class CustomerCreateDTO
    {
        [Required(ErrorMessage = "Customer Code is required.")]
        [MaxLength(4)]
        public string? CustomerCode { get; set; }

        [Required(ErrorMessage = "Customer Name is required.")]
        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }

        public string? Address2 { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string? City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Zip is required.")]
        public string? Zip { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        public string? Phone { get; set; }
    }
}
