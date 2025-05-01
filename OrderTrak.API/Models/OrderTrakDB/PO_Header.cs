using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class PO_Header : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ProjectID { get; set; }

        [Required]
        public string PONumber { get; set; } = string.Empty;

        [ForeignKey("ProjectID")]
        public virtual UPL_Project UPL_Project { get; set; } = null!;

        public virtual ICollection<PO_Line> PO_Line { get; set; } = [];
        public virtual ICollection<ORD_Line> ORD_Line { get; set; } = [];
    }
}
