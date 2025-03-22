namespace OrderTrak.API.Models.DTO.Receiving
{
    public class ReceivingDTO
    {
        public Guid FormID { get; set; }
        public string TrackingNumber { get; set; } = string.Empty;
        public string Carrier { get; set; } = string.Empty;
        public DateTime DataReceived { get; set; }
        public bool CanReceive { get; set; }
        public List<ReceivingLineDTO> ReceivingLines { get; set; } = [];
    }
}
