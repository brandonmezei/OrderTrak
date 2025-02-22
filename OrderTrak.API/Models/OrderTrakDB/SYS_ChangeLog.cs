using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class SYS_ChangeLog : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public virtual ICollection<SYS_ChangeLogDetails> SYS_ChangeLogDetails { get; set; } = new List<SYS_ChangeLogDetails>();
    }
}
