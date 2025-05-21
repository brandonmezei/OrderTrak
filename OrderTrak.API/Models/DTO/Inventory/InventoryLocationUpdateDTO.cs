using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Inventory
{
    public class InventoryLocationUpdateDTO
    {
        [Required]
        public Guid FormID { get; set; }
        public string? LocationNumber { get; set; }
    }
}
