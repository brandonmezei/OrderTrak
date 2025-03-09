using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.StockGroup
{
    public class StockGroupCreateDTO
    {
        [Required(ErrorMessage = "Stock Group Name is required.")]
        public string StockGroupTitle { get; set; } = string.Empty;
    }
}
