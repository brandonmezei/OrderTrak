using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Order
{
    public class OrderCreateDTO
    {
        [Required(ErrorMessage = "Project is required.")]
        public Guid ProjectID { get; set; }
    }
}
