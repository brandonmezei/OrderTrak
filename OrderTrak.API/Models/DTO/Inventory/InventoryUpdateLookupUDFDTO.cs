using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Inventory
{
    public class InventoryUpdateLookupUDFDTO
    {
        [Required]
        public Guid FormID { get; set; }

        public string? UDF1 { get; set; }
        public string? UDF2 { get; set; }
        public string? UDF3 { get; set; }
        public string? UDF4 { get; set; }
        public string? UDF5 { get; set; }
        public string? UDF6 { get; set; }
        public string? UDF7 { get; set; }
        public string? UDF8 { get; set; }
        public string? UDF9 { get; set; }
        public string? UDF10 { get; set; }
    }
}
