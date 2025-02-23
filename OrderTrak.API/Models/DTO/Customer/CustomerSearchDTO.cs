using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Customer
{
    public class CustomerSearchDTO : SearchQueryDTO
    {
        [MaxLength(4)]
        public string? CustomerCode { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zip { get; set; }

        public string? Phone { get; set; }
    }
}
