using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.PO
{
    public class POUpdateLineDTO
    {
        [Required]
        public Guid? FormID { get; set; }

        public string? PartNumber { get; set; }

        public string? PartDescription { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public int? Quantity { get; set; }
    }
}
