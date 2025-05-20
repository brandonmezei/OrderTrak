using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Order
{
    public class OrderCompleteShippingDTO
    {
        [Required]
        public Guid OrderID { get; set; }
    }
}
