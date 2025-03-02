using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Roles
{
    public class RoleToUserSelectDTO
    {
        [Required]
        public Guid? RoleID { get; set; }

        [Required]
        public Guid? UserID { get; set; }
    }
}
