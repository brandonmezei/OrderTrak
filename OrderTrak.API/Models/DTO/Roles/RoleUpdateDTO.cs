using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Roles
{
    public class RoleUpdateDTO
    {
        [Required]
        public Guid? FormID { get; set; }

        [Required]
        public string? RoleName { get; set; } = string.Empty;
    }
}
