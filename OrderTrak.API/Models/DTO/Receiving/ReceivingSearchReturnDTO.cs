namespace OrderTrak.API.Models.DTO.Receiving
{
    public class ReceivingSearchReturnDTO
    {
        public Guid FormID { get; set; }
        public string TrackingNumber { get; set; } = string.Empty;
        public string Carrier { get; set; } = string.Empty;
        public int POCount { get; set; }
        public int QuantityReceived { get; set; }
    }
}
