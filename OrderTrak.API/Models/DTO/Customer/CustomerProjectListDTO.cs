namespace OrderTrak.API.Models.DTO.Customer
{
    public class CustomerProjectListDTO
    {
        public Guid FormID { get; set; }
        public string ProjectCode { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
    }
}
