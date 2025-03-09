namespace OrderTrak.API.Models.DTO.Location
{
    public class LocationSearchReturnDTO
    {
        public Guid FormID { get; set; }
        public string LocationNumber { get; set; } = string.Empty;
        public decimal Volume { get; set; } = 0;
        public string UnitOfMeasure { get; set; } = string.Empty;
    }
}
