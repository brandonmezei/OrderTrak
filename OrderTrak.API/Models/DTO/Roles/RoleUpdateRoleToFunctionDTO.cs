using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Roles
{
    public class RoleUpdateRoleToFunctionDTO
    {
        [Required]
        public Guid? RoleID { get; set; }

        [Required]
        public List<RoleUpdateRoleToFunctionListDTO> UpdateList { get; set; } = [];
    }
}
