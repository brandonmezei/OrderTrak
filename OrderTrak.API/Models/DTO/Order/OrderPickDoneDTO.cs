using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Order
{
    public class OrderPickDoneDTO
    {
        [Required]
        public Guid OrderID { get; set; }
    }
}
