using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Inventory
{
    public class InventoryUpdateLookupDTO
    {
        [Required]
        public Guid FormID { get; set; }
        public Guid? StockGroupID { get; set; }
        public Guid? StatusID { get; set; }
        public string? LocationNumber { get; set; }
        public string? SerialNumber { get; set; }
        public string? AssetTag { get; set; }
    }
}
