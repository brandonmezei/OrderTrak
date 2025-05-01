using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.OrderTrakDB;
using OrderTrak.API.Static;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Services.Inventory
{
    public class InventoryService(OrderTrakContext orderTrakContext) : IInventoryService
    {
        private readonly OrderTrakContext DB = orderTrakContext;

        public async Task<List<INV_Stock>> GetInventoryByOrderLineIDAsync(Guid lineID)
        {
            // Get the OrderLine from the DB
            var orderLine = await DB.ORD_Line
                .FirstOrDefaultAsync(x => x.FormID == lineID)
                ?? throw new ValidationException($"Order Line not found.");

            // Start the Inv_Stock Query
            var invQuery = DB.INV_Stock
                .Include(x => x.PO_Line.PO_Header)
                .Include(x => x.INV_StockStatus)
                .Include(x => x.UPL_StockGroup)
                .Include(x => x.UPL_Location)
                .Include(x => x.INV_Receipt)
                .Where(x => x.PO_Line.PartID == orderLine.PartID
                    && x.INV_StockStatus.StockStatus == StockStatus.InStock
                );

            // Apply PO Line Filter if it exists
            if (orderLine.POHeaderID.HasValue)
                invQuery = invQuery.Where(x => x.PO_Line.POHeaderID == orderLine.POHeaderID);

            // Apply Stock Filter If exists
            if (orderLine.StockGroupID.HasValue)
                invQuery = invQuery.Where(x => x.StockGroupID == orderLine.StockGroupID);

            // Apply Serial Filter if exists
            if (!string.IsNullOrEmpty(orderLine.SerialNumber))
                invQuery = invQuery.Where(x => x.SerialNumber == orderLine.SerialNumber);

            return await invQuery.ToListAsync();
        }

        public async Task<List<ORD_Line>> GetCommittedInventoryByOrderLineIDAsync(Guid lineID)
        {
            // Get the OrderLine from the DB
            var orderLine = await DB.ORD_Line
                .FirstOrDefaultAsync(x => x.FormID == lineID)
                ?? throw new ValidationException($"Order Line not found.");

            // Get Any Order at PickReady Picking with the same part as orderLine ignoring the target order
            var orderQuery = DB.ORD_Line
                .Include(x => x.ORD_Order.ORD_Status)
                .Include(x => x.ORD_PickList)
                    .ThenInclude(x => x.INV_Stock)
                .Where(x => x.OrderID != orderLine.OrderID
                    && x.PartID == orderLine.PartID
                    && (x.ORD_Order.ORD_Status.Status == OrderStatus.PickReady || x.ORD_Order.ORD_Status.Status == OrderStatus.Picking)
                );

            // Apply PO Line Filter if it exists
            if (orderLine.POHeaderID.HasValue)
                orderQuery = orderQuery.Where(x => x.POHeaderID == orderLine.POHeaderID);

            // Apply Stock Filter If exists
            if (orderLine.StockGroupID.HasValue)
                orderQuery = orderQuery.Where(x => x.StockGroupID == orderLine.StockGroupID);

            // Apply Serial Filter if exists
            if (!string.IsNullOrEmpty(orderLine.SerialNumber))
                orderQuery = orderQuery.Where(x => x.SerialNumber == orderLine.SerialNumber);

            return await orderQuery.ToListAsync();
        }

        public async Task<int> GetCommittedQTYByOrderLineIDAsync(Guid lineID)
        {
            var orderLineList = await GetCommittedInventoryByOrderLineIDAsync(lineID);

            var committedQTY = 0;

            foreach(var orderLine in orderLineList)
                committedQTY += orderLine.Quantity - orderLine.ORD_PickList.Sum(x => x.INV_Stock.Quantity);

            return committedQTY;
        }

        public async Task<int> GetInStockQTYByOrderLineIDAsync(Guid lineID)
        {
            var invList = await GetInventoryByOrderLineIDAsync(lineID);

            return invList.Sum(x => x.Quantity);                
        }  
    }
}
