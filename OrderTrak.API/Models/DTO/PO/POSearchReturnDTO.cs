namespace OrderTrak.API.Models.DTO.PO
{
    public class POSearchReturnDTO
    {
        public Guid FormID { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
    }
}
