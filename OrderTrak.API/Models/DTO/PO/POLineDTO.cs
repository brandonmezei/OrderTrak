namespace OrderTrak.API.Models.DTO.PO
{
    public class POLineDTO
    {
        public Guid FormID { get; set; }
        public string PartNumber { get; set; } = string.Empty;
        public string PartDescription { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int RecQuantity { get; set; }
    }
}
