using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class ORD_Order : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]     
        public int ProjectID { get; set; }

        [Required]
        public int StatusID { get; set; }

        public int? StatusIDBeforeHold { get; set; }

        public DateTime? RequestedShipDate { get; set; }
        public DateTime? RequestedDeliveryDate { get; set; }
        public DateTime? ActualShipDate { get; set; }
        public string? StakeHolderEmail { get; set; }
        public string? OrderUDF1 { get; set; }
        public string? OrderUDF2 { get; set; }
        public string? OrderUDF3 { get; set; }
        public string? OrderUDF4 { get; set; }
        public string? OrderUDF5 { get; set; }
        public string? OrderUDF6 { get; set; }
        public string? OrderUDF7 { get; set; }
        public string? OrderUDF8 { get; set; }
        public string? OrderUDF9 { get; set; }
        public string? OrderUDF10 { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? ShipContact { get; set; }
        public string? ShipPhone { get; set; }
        public string? ShipEmail { get; set; }
        public string? Carrier { get; set; }
        public string? OrderNote { get; set; }

        public virtual ICollection<ORD_Line> ORD_Line { get; set; } = [];

        [ForeignKey("ProjectID")]
        public virtual UPL_Project UPL_Project { get; set; } = null!;

        [ForeignKey("StatusID")]
        public virtual ORD_Status ORD_Status { get; set; } = null!;

        [ForeignKey("StatusIDBeforeHold")]
        public virtual ORD_Status ORD_StatusBeforeHold { get; set; } = null!;

        public virtual ICollection<ORD_Tracking> ORD_Tracking { get; set; } = [];
    }
}
