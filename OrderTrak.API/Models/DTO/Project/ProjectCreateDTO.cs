using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Project
{
    public class ProjectCreateDTO
    {
        [Required]
        public Guid? CustID { get; set; }

        [Required(ErrorMessage = "Project Code is required.")]
        [MaxLength(4)]
        public string ProjectCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Project Name is required.")]
        public string ProjectName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact Name is required.")]
        public string ContactName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact Phone is required.")]
        public string ContactPhone { get; set; } = string.Empty;
    }
}
