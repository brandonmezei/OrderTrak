using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Parts
{
    public class PartUpdateDTO
    {
        [Required]
        public Guid? FormID { get; set; }

        [Required(ErrorMessage = "Unit of Measurement is required.")]
        public Guid? UOMID { get; set; }

        [Required(ErrorMessage = "Part Number is required.")]
        public string? PartNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Part Description is required.")]
        public string? PartDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Part Type is required.")]
        public string? PartType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Part Vendor is required.")]
        public string? PartVendor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Part Cost is required.")]
        [DataType(DataType.Currency)]
        public decimal? PartCost { get; set; } = 0;

        [Required]
        public bool IsStock { get; set; } = false;

        public decimal? Height { get; set; }

        public decimal? Width { get; set; }

        public decimal? Depth { get; set; }
    }
}
