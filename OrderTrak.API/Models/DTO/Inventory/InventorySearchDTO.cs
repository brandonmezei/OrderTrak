namespace OrderTrak.API.Models.DTO.Inventory
{
    public class InventorySearchDTO : SearchQueryDTO
    {
       public Guid? OrderLineID { get; set; }
    }
}
