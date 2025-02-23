using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO
{
    public class SearchQueryDTO
    {
        [Required]
        [Range(0, 50, ErrorMessage = "Page must be between 0 and 50.")]
        public int RecordSize { get; set; } = 1;

        [Required]
        public int Page { get; set; } = 1;
    }
}
