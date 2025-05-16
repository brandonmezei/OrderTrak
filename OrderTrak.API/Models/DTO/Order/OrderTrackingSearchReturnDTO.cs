namespace OrderTrak.API.Models.DTO.Order
{
    public class OrderTrackingSearchReturnDTO
    {
        public Guid FormID { get; set; }
        public string Tracking { get; set; } = string.Empty;
        public int BoxCount { get; set; } = 0;
        public decimal Weight { get; set; } = 0;
        public int? PalletCount { get; set; }
    }
}
