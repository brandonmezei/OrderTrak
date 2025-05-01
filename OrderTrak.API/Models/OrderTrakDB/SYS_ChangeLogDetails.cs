using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class SYS_ChangeLogDetails : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]       
        public int ChangeLogId { get; set; }

        [Required]
        public int TicketID { get; set; }

        [Required]
        public string TicketInfo { get; set; } = string.Empty;

        [ForeignKey("ChangeLogId")]
        public virtual SYS_ChangeLog SYS_ChangeLog { get; set; } = null!;
    }
}
