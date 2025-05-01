namespace OrderTrak.API.Models.DTO.Order
{
    public class OrderPartListDTO
    {
        public Guid FormID { get; set; }
        public Guid PartID { get; set; }
        public Guid? POID { get; set; }
        public Guid? StockGroupID { get; set; }
        public string PartNumber { get; set; } = string.Empty;
        public string PartDescription { get; set; } = string.Empty;
        public string? PO { get; set; }
        public string? StockGroup { get; set; }
        public string? SerialNumber { get; set; }
        public int Quantity { get; set; }
        public int PickedQuantity { get; set; }
        public int CommittedQuantity { get; set; }
        public int InStockQuantity { get; set; }
    }
}
