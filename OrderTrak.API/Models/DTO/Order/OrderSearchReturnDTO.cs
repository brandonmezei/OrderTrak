namespace OrderTrak.API.Models.DTO.Order
{
    public class OrderSearchReturnDTO
    {
        public Guid FormID { get; set; }
        public int OrderID { get; set; }
        public string ProjectCode { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;
        public DateTime? RequestedShipDate { get; set; }
        public DateTime? RequestedDeliveryDate { get; set; }
        public DateTime? ActualShipDate { get; set; }
    }
}
