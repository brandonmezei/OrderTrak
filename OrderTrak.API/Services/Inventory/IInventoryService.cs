using OrderTrak.API.Models.OrderTrakDB;

namespace OrderTrak.API.Services.Inventory
{
    public interface IInventoryService
    {
        Task<List<INV_Stock>> GetInventoryByOrderLineIDAsync(Guid lineID);
        Task<List<ORD_Line>> GetCommittedInventoryByOrderLineIDAsync(Guid lineID);
        Task<int> GetCommittedQTYByOrderLineIDAsync(Guid lineID);
        Task<int> GetInStockQTYByOrderLineIDAsync(Guid lineID);
    }
}
