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
        [ForeignKey("UPL_PartInfo")]
        public int PartID { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public bool IsSerialized { get; set; }

        public virtual PO_Header PO_Header { get; set; } = null!;
        public virtual UPL_PartInfo UPL_PartInfo { get; set; } = null!;
        public virtual ICollection<INV_Stock> INV_Stock { get; set; } = [];
        public virtual ICollection<ORD_Line> ORD_Line { get; set; } = [];
    }
}
