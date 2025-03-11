using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class UPL_UOM : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string UnitOfMeasurement { get; set; } = string.Empty;

        public virtual ICollection<UPL_PartInfo> UPL_PartInfo { get; set; } = [];
        public virtual ICollection<UPL_Location> UPL_Location { get; set; } = [];

    }
}
