using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class UPL_PartInfo : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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

        [Required]
        public string PartUnit { get; set; } = "EA";

        [Required]
        public bool IsStock { get; set; } = false;

        public virtual ICollection<UPL_ProjectPart> UPL_ProjectPart { get; set; } = [];
    }
}
