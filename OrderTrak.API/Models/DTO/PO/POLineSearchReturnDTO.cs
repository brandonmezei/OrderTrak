namespace OrderTrak.API.Models.DTO.PO
{
    public class POLineSearchReturnDTO
    {
        public Guid FormID { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int RecQuantity { get; set; }
        public bool IsSerialized { get; set; }
    }
}
