using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class UPL_Project : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]    
        public int CustomerID { get; set; }

        [Required]
        [MaxLength(4)]
        public string ProjectCode { get; set; } = string.Empty;

        [Required]
        public string ProjectName { get; set; } = string.Empty;

        [Required]
        public string ContactName { get; set; } = string.Empty;

        [Required]
        public string ContactPhone { get; set; } = string.Empty;

        public string? ContactEmail { get; set; }
        public string? StakeHolderEmail { get; set; }
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

        [ForeignKey("CustomerID")]
        public virtual UPL_Customer UPL_Customer { get; set; } = null!;
        public virtual ICollection<ORD_Order> ORD_Order { get; set; } = [];
    }
}
