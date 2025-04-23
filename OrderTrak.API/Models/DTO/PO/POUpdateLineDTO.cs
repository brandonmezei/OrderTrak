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
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int? Quantity { get; set; }

        [Required]
        public bool IsSerialized { get; set; }
    }
}
