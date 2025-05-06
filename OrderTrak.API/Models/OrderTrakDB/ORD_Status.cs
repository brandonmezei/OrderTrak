using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class ORD_Status : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;

        [InverseProperty(nameof(ORD_Order.ORD_Status))]
        public virtual ICollection<ORD_Order> ORD_Orders { get; set; } = [];

        [InverseProperty(nameof(ORD_Order.ORD_StatusBeforeHold))]
        public virtual ICollection<ORD_Order> ORD_OrdersBeforeHold { get; set; } = [];
    }
}
