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
        public int OrderID { get; set; }

        [Required]
        public int PartID { get; set; }

        public int? POHeaderID { get; set; }

        public int? StockGroupID { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string? SerialNumber { get; set; }

        [ForeignKey("OrderID")]
        public virtual ORD_Order ORD_Order { get; set; } = null!;

        [ForeignKey("PartID")]
        public virtual UPL_PartInfo UPL_PartInfo { get; set; } = null!;

        [ForeignKey("POHeaderID")]
        public virtual PO_Header? PO_Header { get; set; } = null!;

        [ForeignKey("StockGroupID")]
        public virtual UPL_StockGroup? UPL_StockGroup { get; set; } = null!;
        public virtual ICollection<ORD_PickList> ORD_PickList { get; set; } = [];
    }
}
