namespace OrderTrak.API.Models.DTO.Inventory
{
    public class InventorySearchReturnDTO
    {
        public Guid FormID { get; set; }
        public Guid ProjectID { get; set; }
        public Guid? StockGroupID { get; set; }
        public Guid? StatusID { get; set; }
        public int BoxID { get; set; }
        public string ProjectCode { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string PartDescription { get; set; } = string.Empty;
        public string LocationNumber { get; set; } = string.Empty;
        public string Status { get;set; } = string.Empty;
        public string? PO { get; set; }
        public string? StockGroup { get; set; }
        public string? SerialNumber { get; set; }
        public string? AssetTag { get; set; }
        public int Quantity { get; set; }
        public string? UDF1 { get; set; }
        public string? UDF2 { get; set; }
        public string? UDF3 { get; set; }
        public string? UDF4 { get; set; }
        public string? UDF5 { get; set; }
        public string? UDF6 { get; set; }
        public string? UDF7 { get; set; }
        public string? UDF8 { get; set; }
        public string? UDF9 { get; set; }
        public string? UDF10 { get; set; }
        public string? UDFLabel1 { get; set; }
        public string? UDFLabel2 { get; set; }
        public string? UDFLabel3 { get; set; }
        public string? UDFLabel4 { get; set; }
        public string? UDFLabel5 { get; set; }
        public string? UDFLabel6 { get; set; }
        public string? UDFLabel7 { get; set; }
        public string? UDFLabel8 { get; set; }
        public string? UDFLabel9 { get; set; }
        public string? UDFLabel10 { get; set; }
        public bool CanUpdate { get; set; }
        public bool IsSerialized { get; set; }
    }
}
