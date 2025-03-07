namespace OrderTrak.API.Models.DTO.Parts
{
    public class PartSearchDTO : SearchQueryDTO
    {
        public bool IsStockOnly { get; set; }
    }
}
