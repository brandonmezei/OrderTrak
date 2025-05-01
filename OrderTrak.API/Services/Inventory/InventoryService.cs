using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.OrderTrakDB;

namespace OrderTrak.API.Services.Inventory
{
    public class InventoryService(OrderTrakContext orderTrakContext) : IInventoryService
    {
        private readonly OrderTrakContext DB = orderTrakContext;

        public Task<int> GetCommittedQTYByOrderLineIDAsync(Guid lineID)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetInStockQTYByOrderLineIDAsync(Guid lineID)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<INV_Stock>> GetInventoryQueryByOrderLineIDAsync(Guid lineID)
        {
            // Get the OrderLine from the DB
            var orderLine = await DB.ORD_Line
                .FirstOrDefaultAsync(x => x.FormID == lineID)
                ?? throw new Exception($"Order Line not found.");

            // Start the Inv_Stock Query
            var invQuery = DB.INV_Stock
                .Include(x => x.PO_Line.PO_Header)
                .Include(x => x.INV_StockStatus)
                .Include(x => x.UPL_StockGroup)
                .Include(x => x.UPL_Location)
                .Include(x => x.INV_Receipt)
                .Where(x => x.PO_Line.PartID == orderLine.PartID);

            // Apply PO Line Filter if it exists
            if (orderLine.POHeaderID.HasValue)
                invQuery = invQuery.Where(x => x.PO_Line.POHeaderID == orderLine.POHeaderID);

            // Apply Stock Filter If exists
            if (orderLine.StockGroupID.HasValue)
                invQuery = invQuery.Where(x => x.StockGroupID == orderLine.StockGroupID);

            // Apply Serial Filter if exists
            if (orderLine.SerialNumber != null)
                invQuery = invQuery.Where(x => x.SerialNumber == orderLine.SerialNumber);

            return invQuery;
        }
    }
}
