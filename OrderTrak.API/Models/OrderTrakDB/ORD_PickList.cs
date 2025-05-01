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
        public int LineID { get; set; }

        [Required]
        public int StockID { get; set; }

        [ForeignKey("LineID")]
        public virtual ORD_Line ORD_Line { get; set; } = null!;

        [ForeignKey("StockID")]
        public virtual INV_Stock INV_Stock { get; set; } = null!;
    }
}
