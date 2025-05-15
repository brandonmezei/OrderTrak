using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Order
{
    public class OrderPickDTO
    {
        [Required]
        public Guid OrderLineID { get; set; }

        [Required]
        public Guid InventoryID { get; set; }
    }
}
