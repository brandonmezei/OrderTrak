namespace OrderTrak.API.Models.DTO.Location
{
    public class LocationDTO
    {
        public Guid FormID { get; set; }
        public Guid UOMID { get; set; }
        public string LocationNumber { get; set; } = string.Empty;
        public decimal Height { get; set; } = 0;
        public decimal Width { get; set; } = 0;
        public decimal Depth { get; set; } = 0;
    }
}
