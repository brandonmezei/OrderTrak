using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class SYS_ChangeLogDetails : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("SYS_ChangeLog")]
        public int ChangeLogId { get; set; }

        [Required]
        public int TicketID { get; set; }

        [Required]
        public string TicketInfo { get; set; } = string.Empty;

        public virtual SYS_ChangeLog SYS_ChangeLog { get; set; } = null!;
    }
}
