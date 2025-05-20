namespace OrderTrak.API.Models.DTO.Receiving
{
    public class ReceivingPutawaySearchReturnDTO
    {
        public Guid FormID { get; set; }
        public int BoxID { get; set; }
        public string PartNumber { get; set; } = string.Empty;
        public string PartDescription { get; set; } = string.Empty;
        public string PO { get; set; } = string.Empty;
        public string StockGroup { get; set; } = string.Empty;
        public string Location { get; set;} = string.Empty;
        public int Quantity { get; set; } = 0;
    }
}
