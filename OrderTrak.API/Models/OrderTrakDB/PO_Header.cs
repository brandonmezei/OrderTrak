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
        [ForeignKey("UPL_Project")]
        public int ProjectID { get; set; }

        [Required]
        public string PONumber { get; set; } = string.Empty;

        public virtual UPL_Project UPL_Project { get; set; } = null!;

        public virtual ICollection<PO_Line> PO_Line { get; set; } = [];
    }
}
