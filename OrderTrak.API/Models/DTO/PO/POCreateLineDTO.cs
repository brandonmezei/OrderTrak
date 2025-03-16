using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.PO
{
    public class POCreateLineDTO
    {
        [Required]
        public Guid OrderID { get; set; }
        
        [Required]
        public Guid PartID { get; set; }
    }
}
