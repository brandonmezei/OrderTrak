using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class UPL_PartInfo : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("UPL_UOM")]
        public int UOMID { get; set; }

        [Required]
        public string PartNumber { get; set; } = string.Empty;

        [Required]
        public string PartDescription { get; set; } = string.Empty;

        [Required]
        public string PartType { get; set; } = string.Empty;

        [Required]
        public string PartVendor { get; set; } = string.Empty;

        [Required]
        public decimal PartCost { get; set; } = 0;

        public decimal? Height { get; set; }

        public decimal? Width { get; set; }

        public decimal? Depth { get; set; }

        [Required]
        public bool IsStock { get; set; } = false;

        public virtual ICollection<PO_Line> PO_Line { get; set; } = [];

        public virtual UPL_UOM UPL_UOM { get; set; } = null!;

    }
}
