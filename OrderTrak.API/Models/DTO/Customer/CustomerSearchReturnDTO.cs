namespace OrderTrak.API.Models.DTO.Customer
{
    public class CustomerSearchReturnDTO
    {
        public Guid FormID { get; set; }
        public string CustomerCode { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public int ProjectCount { get; set; }
    }
}
