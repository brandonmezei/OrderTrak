using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class UPL_Location : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string LocationNumber { get; set; } = string.Empty;

        [Required]
        public decimal Height { get; set; } = 0;

        [Required]
        public decimal Width { get; set; } = 0;

        [Required]
        public decimal Depth { get; set; } = 0;

        [Required]
        public string UnitOfMeasure { get; set; } = string.Empty;
    }
}
