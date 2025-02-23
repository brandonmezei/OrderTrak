using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class PO_Line : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("PO_Header")]
        public int POHeaderID { get; set; }

        [Required]
        [ForeignKey("UPL_ProjectPart")]
        public int ProjectPartID { get; set; }

        [Required]
        public int Quantity { get; set; }

        public virtual PO_Header PO_Header { get; set; } = null!;
        public virtual UPL_ProjectPart UPL_ProjectPart { get; set; } = null!;
    }
}
