using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class CommonObject
    {
        [Required]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(50)]
        public string CreateName { get; set; } = string.Empty;

        public DateTime? UpdateDate { get; set; }

        [MaxLength(50)]
        public string? UpdateName { get; set; }

        [Required]
        public bool IsDelete { get; set; }

        [Required]
        public Guid FormID { get; set; } = Guid.NewGuid();
    }
}
