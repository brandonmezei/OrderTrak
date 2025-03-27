using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class INV_Stock : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("INV_Receipt")]
        public int ReceiptID { get; set; }

        [Required]
        [ForeignKey("PO_Line")]
        public int POLineID { get; set; }

        [Required]
        [ForeignKey("INV_StockStatus")]
        public int StatusID { get; set; }

        [Required]
        [ForeignKey("UPL_StockGroup")]
        public int StockGroupID { get; set; }

        [Required]
        [ForeignKey("UPL_Location")]
        public int LocationID { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string? SerialNumber { get; set; }
        public string? AssetTag { get; set; }
        public string? UDF1 { get; set; }
        public string? UDF2 { get; set; }
        public string? UDF3 { get; set; }
        public string? UDF4 { get; set; }
        public string? UDF5 { get; set; }
        public string? UDF6 { get; set; }
        public string? UDF7 { get; set; }
        public string? UDF8 { get; set; }
        public string? UDF9 { get; set; }
        public string? UDF10 { get; set; }

        public virtual INV_Receipt INV_Receipt { get; set; } = null!;

        public virtual PO_Line PO_Line { get; set; } = null!;

        public virtual UPL_StockGroup UPL_StockGroup { get; set; } = null!;

        public virtual UPL_Location UPL_Location { get; set; } = null!;

        public virtual INV_StockStatus INV_StockStatus { get; set; } = null!;
    }
}
