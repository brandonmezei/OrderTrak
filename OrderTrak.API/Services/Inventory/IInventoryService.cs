using OrderTrak.API.Models.OrderTrakDB;

namespace OrderTrak.API.Services.Inventory
{
    public interface IInventoryService
    {
        Task<IQueryable<INV_Stock>> GetInventoryQueryByOrderLineIDAsync(Guid lineID);
        Task<int> GetCommittedQTYByOrderLineIDAsync(Guid lineID);
        Task<int> GetInStockQTYByOrderLineIDAsync(Guid lineID);
    }
}
