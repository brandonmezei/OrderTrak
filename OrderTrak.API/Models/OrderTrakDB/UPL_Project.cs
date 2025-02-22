using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class UPL_Project : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("UPL_Customer")]
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

        public string? ContactEmail { get; set; } = string.Empty;

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

        public virtual UPL_Customer UPL_Customer { get; set; } = null!;
    }
}
