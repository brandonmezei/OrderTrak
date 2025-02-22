using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class UPL_Customer : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(4)]
        public string CustomerCode { get; set; } = string.Empty;

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        public string? Address2 { get; set; }

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        public string Zip { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        public virtual ICollection<UPL_Project> UPL_Projects { get; set; } = [];
    }
}
