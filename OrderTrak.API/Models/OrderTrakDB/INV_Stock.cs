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
        public int ReceiptID { get; set; }

        [Required]
        public int POLineID { get; set; }

        [Required]
        public int StatusID { get; set; }

        [Required]
        public int StockGroupID { get; set; }

        [Required]
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

        [ForeignKey("ReceiptID")]
        public virtual INV_Receipt INV_Receipt { get; set; } = null!;

        [ForeignKey("POLineID")]
        public virtual PO_Line PO_Line { get; set; } = null!;

        [ForeignKey("StockGroupID")]
        public virtual UPL_StockGroup UPL_StockGroup { get; set; } = null!;

        [ForeignKey("LocationID")]
        public virtual UPL_Location UPL_Location { get; set; } = null!;

        [ForeignKey("StatusID")]
        public virtual INV_StockStatus INV_StockStatus { get; set; } = null!;
        public virtual ICollection<ORD_PickList> ORD_PickList { get; set; } = [];
    }
}
