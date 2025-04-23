using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Receiving
{
    public class ReceivingLineCreateDTO
    {
        [Required]
        public Guid? RecID { get; set; }

        [Required]
        public Guid? PoLineID { get; set; }

        [Required]
        public Guid? StockGroupID { get; set; }

        public List<ReceivingBoxLineCreateDTO> BoxLineList { get; set; } = [];
    }
}
