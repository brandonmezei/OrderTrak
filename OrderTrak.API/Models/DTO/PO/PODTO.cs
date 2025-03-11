namespace OrderTrak.API.Models.DTO.PO
{
    public class PoDTO
    {
        public Guid FormID { get; set; }
        public Guid ProjectID { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public List<POLineDTO> POLines { get; set; } = [];
    }
}
