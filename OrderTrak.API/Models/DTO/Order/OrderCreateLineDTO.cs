using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Order
{
    public class OrderCreateLineDTO
    {
        [Required]
        public Guid OrderID { get; set; }

        [Required]
        public Guid PartID { get; set; }
    }
}
