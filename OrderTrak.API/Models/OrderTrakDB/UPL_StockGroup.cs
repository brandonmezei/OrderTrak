using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class UPL_StockGroup : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string StockGroupTitle { get; set; } = string.Empty;

        public virtual ICollection<INV_Stock> INV_Stock { get; set; } = [];
        public virtual ICollection<ORD_Line> ORD_Line { get; set; } = [];
    }
}
