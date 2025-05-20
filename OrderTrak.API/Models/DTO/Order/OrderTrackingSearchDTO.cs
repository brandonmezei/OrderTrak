namespace OrderTrak.API.Models.DTO.Order
{
    public class OrderTrackingSearchDTO : SearchQueryDTO
    {
        public Guid? OrderID { get; set; }
    }
}
