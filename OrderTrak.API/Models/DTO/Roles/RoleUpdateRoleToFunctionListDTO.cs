using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Roles
{
    public class RoleUpdateRoleToFunctionListDTO
    {
        [Required]
        public Guid? FormID { get; set; }

        public bool CanAccess { get; set; }
    }
}
