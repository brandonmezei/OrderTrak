using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Roles
{
    public class RoleCreateDTO
    {
        [Required]
        public string RoleName { get; set; } = string.Empty;
    }
}
