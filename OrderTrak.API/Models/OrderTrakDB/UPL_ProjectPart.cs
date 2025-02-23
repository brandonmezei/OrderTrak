using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class UPL_ProjectPart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("UPL_Project")]
        public int ProjectID { get; set; }

        [Required]
        [ForeignKey("UPL_PartInfo")]
        public int PartID { get; set; }

        [Required]
        public bool Serial { get; set; }

        [Required]
        public bool Asset { get; set; }

        [Required]
        public bool UDF1Visible { get; set; }

        [Required]
        public bool UDF2Visible { get; set; }

        [Required]
        public bool UDF3Visible { get; set; }

        [Required]
        public bool UDF4Visible { get; set; }

        [Required]
        public bool UDF5Visible { get; set; }

        [Required]
        public bool UDF6Visible { get; set; }

        [Required]
        public bool UDF7Visible { get; set; }

        [Required]
        public bool UDF8Visible { get; set; }

        [Required]
        public bool UDF9Visible { get; set; }

        [Required]
        public bool UDF10Visible { get; set; }

        [Required]
        public bool UDF2Mandatory { get; set; }

        [Required]
        public bool UDF3Mandatory { get; set; }

        [Required]
        public bool UDF4Mandatory { get; set; }

        [Required]
        public bool UDF5Mandatory { get; set; }

        [Required]
        public bool UDF6Mandatory { get; set; }

        [Required]
        public bool UDF7Mandatory { get; set; }

        [Required]
        public bool UDF8Mandatory { get; set; }

        [Required]
        public bool UDF9Mandatory { get; set; }

        [Required]
        public bool UDF10Mandatory { get; set; }

        public virtual UPL_Project UPL_Project { get; set; } = null!;
        public virtual UPL_PartInfo UPL_PartInfo { get; set; } = null!;
        public virtual ICollection<PO_Line> PO_Line { get; set; } = [];
    }
}
