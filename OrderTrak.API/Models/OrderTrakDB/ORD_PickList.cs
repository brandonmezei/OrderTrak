using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class ORD_PickList : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("ORD_Line")]
        public int LineID { get; set; }

        [Required]
        [ForeignKey("INV_Stock")]
        public int StockID { get; set; }

        public virtual ORD_Line ORD_Line { get; set; } = null!;
        public virtual INV_Stock INV_Stock { get; set; } = null!;
    }
}
