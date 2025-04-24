namespace OrderTrak.API.Models.DTO.Receiving
{
    public class ReceivingLineDTO
    {
        public string PartNumber { get; set; } = string.Empty;
        public string PartDescription { get; set; } = string.Empty;
        public string PurchaseOrder { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
