namespace OrderTrak.API.Models.DTO.Parts
{
    public class PartSearchReturnDTO
    {
        public Guid FormID { get; set; }
        public string PartNumber { get; set; } = string.Empty;
        public string PartDescription { get; set; } = string.Empty;
        public string PartType { get; set; } = string.Empty;
        public bool IsStock { get; set; }
    }
}
