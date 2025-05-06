using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Order
{
    public class OrderShipUpdateDTO
    {
        [Required]
        public Guid FormID { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? ShipContact { get; set; }
        public string? ShipPhone { get; set; }
        public string? ShipEmail { get; set; }
        public string? Carrier { get; set; }
    }
}
