namespace OrderTrak.API.Models.DTO.Order
{
    public class OrderHeaderDTO
    {
        public Guid FormID { get; set; }

        public Guid ProjectID { get; set; }

        public int OrderID { get; set; }
        public string ProjectCode { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty; 
        public DateTime? RequestedShipDate { get; set; }
        public DateTime? RequestedDeliveryDate { get; set; }
        public DateTime? ActualShipDate { get; set; }
        public string? StakeHolderEmail { get; set; }
        public string? OrderUDF1 { get; set; }
        public string? OrderUDF2 { get; set; }
        public string? OrderUDF3 { get; set; }
        public string? OrderUDF4 { get; set; }
        public string? OrderUDF5 { get; set; }
        public string? OrderUDF6 { get; set; }
        public string? OrderUDF7 { get; set; }
        public string? OrderUDF8 { get; set; }
        public string? OrderUDF9 { get; set; }
        public string? OrderUDF10 { get; set; }
    }
}
