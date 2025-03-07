using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Parts
{
    public class PartDTO
    {
        public Guid FormID { get; set; }
        public string PartNumber { get; set; } = string.Empty;
        public string PartDescription { get; set; } = string.Empty;
        public string PartType { get; set; } = string.Empty;
        public string PartVendor { get; set; } = string.Empty;
        public decimal PartCost { get; set; } = 0;
        public string PartUnit { get; set; } = "EA";
        public bool IsStock { get; set; } = false;
    }
}
