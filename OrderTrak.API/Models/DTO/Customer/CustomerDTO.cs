using OrderTrak.API.Models.DTO.Project;

namespace OrderTrak.API.Models.DTO.Customer
{
    public class CustomerDTO
    {
        public Guid FormID { get; set; }
        public string CustomerCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Address2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
