namespace OrderTrak.API.Models.DTO.Receiving
{
    public class ReceivingSearchDTO : SearchQueryDTO
    {
        public bool IsEmpty { get; set; }
        public bool IsToday { get; set; } = true;
    }
}
