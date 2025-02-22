using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class SYS_Roles : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string RoleName { get; set; } = string.Empty;

        public virtual ICollection<SYS_RolesToFunction> SYS_RolesToFunction { get; set; } = [];
    }
}
