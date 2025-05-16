using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class ORD_Tracking : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public string Tracking { get; set; } = string.Empty;

        [Required]
        public int BoxCount { get; set; } = 0;

        [Required]
        public decimal Weight { get; set; } = 0;

        public int? PalletCount { get; set; }

        [ForeignKey("OrderID")]
        public virtual ORD_Order ORD_Order { get; set; } = null!;
    }
}
