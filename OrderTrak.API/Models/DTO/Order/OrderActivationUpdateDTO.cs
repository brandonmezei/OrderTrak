using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Order
{
    public class OrderActivationUpdateDTO
    {
        [Required]
        public Guid FormID { get; set; }
        public Guid? StatusID { get; set; }
        public string? OrderNote { get; set; }
    }
}
