using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class ORD_Line : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("ORD_Order")]
        public int OrderID { get; set; }

        [Required]
        [ForeignKey("UPL_PartInfo")]
        public int PartID { get; set; }

        [ForeignKey("PO_Line")]
        public int POLineID { get; set; }

        [ForeignKey("UPL_StockGroup")]
        public int StockGroupID { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string? SerialNumber { get; set; }

        public virtual ORD_Order ORD_Order { get; set; } = null!;
        public virtual UPL_PartInfo UPL_PartInfo { get; set; } = null!;
        public virtual PO_Line? PO_Line { get; set; } = null!;
        public virtual UPL_StockGroup? UPL_StockGroup { get; set; } = null!;
        public virtual ICollection<ORD_PickList> ORD_PickList { get; set; } = [];
    }
}
