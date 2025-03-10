namespace OrderTrak.API.Models.DTO.Parts
{
    public class PartDTO
    {
        public Guid FormID { get; set; }
        public Guid UOMID { get; set; }
        public string PartNumber { get; set; } = string.Empty;
        public string PartDescription { get; set; } = string.Empty;
        public string PartType { get; set; } = string.Empty;
        public string PartVendor { get; set; } = string.Empty;
        public decimal PartCost { get; set; } = 0;
        public bool IsStock { get; set; } = false;
        public decimal? Height { get; set; }

        public decimal? Width { get; set; }

        public decimal? Depth { get; set; }
    }
}
