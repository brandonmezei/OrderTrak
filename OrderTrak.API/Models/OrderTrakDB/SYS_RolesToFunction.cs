using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class SYS_RolesToFunction : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("SYS_Roles")]
        public int RoleID { get; set; }

        [Required]
        [ForeignKey("SYS_Function")]
        public int FunctionID { get; set; }

        [Required]
        public bool CanAccess { get; set; }

        public virtual SYS_Roles SYS_Roles { get; set; } = null!;
        public virtual SYS_Function SYS_Function { get; set; } = null!;
    }
}
