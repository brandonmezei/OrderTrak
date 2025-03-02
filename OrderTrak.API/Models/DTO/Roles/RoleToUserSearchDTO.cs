using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Roles
{
    public class RoleToUserSearchDTO : SearchQueryDTO
    {
        [Required]
        public Guid? FormID { get; set; }
    }
}
