using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Receiving
{
    public class ReceivingBoxLineCreateDTO
    {
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int? Quantity { get; set; }

        public string? SerialNumber { get; set; }
        public string? AssetTag { get; set; }

        public List<ReferencesDTO> UDFList { get; set; } = [];
    }
}
