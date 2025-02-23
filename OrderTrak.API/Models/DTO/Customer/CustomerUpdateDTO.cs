using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Customer
{
    public class CustomerUpdateDTO
    {
        [Required]
        public Guid FormID { get; set; }

        [Required]
        [MaxLength(4)]
        public string CustomerCode { get; set; } = string.Empty;

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        public string? Address2 { get; set; }

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        public string Zip { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;
    }
}
