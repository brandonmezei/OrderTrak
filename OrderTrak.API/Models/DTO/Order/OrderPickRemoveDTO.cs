using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Order
{
    public class OrderPickRemoveDTO
    {
        [Required]
        public Guid OrderLineID { get; set; }

        [Required]
        public Guid InventoryID { get; set; }
    }
}
