namespace OrderTrak.API.Models.DTO.Order
{
    public class OrderShipDTO
    {
        public Guid FormID { get; set; }

        public Guid ProjectID { get; set; }

        public int OrderID { get; set; }
        public string CustomerCode { get; set; } = string.Empty;
        public string ProjectCode { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;

        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? ShipContact { get; set; }
        public string? ShipPhone { get; set; }
        public string? ShipEmail { get; set; }
        public string? Carrier { get; set; }
    }
}
