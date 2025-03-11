using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.PO
{
    public class POCreateDTO
    {

        [Required(ErrorMessage = "Project is required.")]
        public Guid ProjectID { get; set; }

        [Required(ErrorMessage = "PO Number is required.")]
        public string PONumber { get; set; } = string.Empty;
    }
}
