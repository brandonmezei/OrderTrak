namespace OrderTrak.API.Models.DTO.ChangeLog
{
    public class ChangeLogDTO
    {
        public DateTime RollOutDate { get; set; } = DateTime.Now;

        public List<ChangeLogDetailsDTO> ChangeLogDetails { get; set; } = [];
    }
}
