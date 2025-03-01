using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Roles
{
    public class RoleCreateDTO
    {
        [Required(ErrorMessage = "Role Name is required.")]
        public string RoleName { get; set; } = string.Empty;
    }
}
