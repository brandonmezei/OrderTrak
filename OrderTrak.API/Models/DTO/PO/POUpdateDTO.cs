using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.PO
{
    public class POUpdateDTO
    {
        [Required]
        public Guid? FormID { get; set; }

        [Required(ErrorMessage = "PO Number is required.")]
        public string? PONumber { get; set; } = string.Empty;
    }
}
