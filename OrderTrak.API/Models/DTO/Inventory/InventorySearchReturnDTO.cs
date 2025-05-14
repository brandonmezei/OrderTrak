namespace OrderTrak.API.Models.DTO.Inventory
{
    public class InventorySearchReturnDTO
    {
        public Guid FormID { get; set; }
        public int BoxID { get; set; }
        public string ProjectCode { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string PartDescription { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string? PO { get; set; }
        public string? StockGroup { get; set; }
        public string? SerialNumber { get; set; }
        public int Quantity { get; set; }
    }
}
