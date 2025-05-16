using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Order
{
    public class OrderCreateTrackingDTO
    {
        [Required(ErrorMessage = "Order ID is required.")]
        public Guid FormID { get; set; }

        [Required(ErrorMessage = "Tracking Number is required.")]
        public string TrackingNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Box Count is required.")]
        public int BoxCount { get; set; } = 0;

        [Required(ErrorMessage = "Weight is required.")]
        public decimal Weight { get; set; } = 0;

        public int? PalletCount { get; set; }
    }
}
