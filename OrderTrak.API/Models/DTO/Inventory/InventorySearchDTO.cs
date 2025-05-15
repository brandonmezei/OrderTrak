namespace OrderTrak.API.Models.DTO.Inventory
{
    public class InventorySearchDTO : SearchQueryDTO
    {
       public Guid? OrderLineID { get; set; }
       public Guid? InventoryID { get; set; }

       public bool ShowPickedOnly { get; set; }
    }
}
