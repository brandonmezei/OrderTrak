namespace OrderTrak.API.Models.DTO.PO
{
    public class POLineSearchReturnDTO
    {
        public Guid FormID { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public int RecQuantity { get; set; } = 0;
        public bool IsSerialized { get; set; }
        public string? UDF1 { get; set; } = string.Empty;
        public string? UDF2 { get; set; } = string.Empty;
        public string? UDF3 { get; set; } = string.Empty;
        public string? UDF4 { get; set; } = string.Empty;
        public string? UDF5 { get; set; } = string.Empty;
        public string? UDF6 { get; set; } = string.Empty;
        public string? UDF7 { get; set; } = string.Empty;
        public string? UDF8 { get; set; } = string.Empty;
        public string? UDF9 { get; set; } = string.Empty;
        public string? UDF10 { get; set; } = string.Empty;
    }
}
