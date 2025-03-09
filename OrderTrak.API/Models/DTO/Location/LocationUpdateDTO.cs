using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Location
{
    public class LocationUpdateDTO
    {
        [Required]
        public Guid? FormID { get; set; }

        [Required(ErrorMessage = "Location Number is required.")]
        public string? LocationNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Height is required.")]
        public decimal? Height { get; set; } = 0;

        [Required(ErrorMessage = "Width is required.")]
        public decimal? Width { get; set; } = 0;

        [Required(ErrorMessage = "Depth is required.")]
        public decimal? Depth { get; set; } = 0;

        [Required(ErrorMessage = "Unit of Measurement is required.")]
        public string? UnitOfMeasure { get; set; } = string.Empty;
    }
}
