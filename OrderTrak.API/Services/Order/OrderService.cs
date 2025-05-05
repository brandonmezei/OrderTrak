using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Order;
using OrderTrak.API.Models.OrderTrakDB;
using OrderTrak.API.Static;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Services.Order
{
    public class OrderService(OrderTrakContext orderTrakContext) : IOrderService
    {
        private readonly OrderTrakContext DB = orderTrakContext;

        private async Task PlaceOrderOnHoldAsync(Guid orderID)
        {
            // Get Order making sure it's not shipped or draft or already on hold
            var order = await DB.ORD_Order
                .FirstOrDefaultAsync(x => x.FormID == orderID
                    && x.ORD_Status.Status != OrderStatus.Shipped
                    && x.ORD_Status.Status != OrderStatus.Draft
                    && x.ORD_Status.Status != OrderStatus.Hold
                    );

            if (order != null)
            {
                // Get Hold Status
                var holdStatus = await DB.ORD_Status
                    .FirstOrDefaultAsync(x => x.Status == OrderStatus.Hold)
                    ?? throw new ValidationException("Cannot find Hold Status");

                // Update Order
                order.ORD_Status = holdStatus;

                // Save
                await DB.SaveChangesAsync();
            }
        }

        public async Task<Guid> CreateOrderAsync(OrderCreateDTO orderCreateDTO)
        {
            // Get Project
            var project = await DB.UPL_Project.FirstOrDefaultAsync(x => x.FormID == orderCreateDTO.ProjectID)
                ?? throw new ValidationException("Project not found.");

            // Get Draft Ord_Status
            var draftStatus = await DB.ORD_Status
                .FirstOrDefaultAsync(x => x.Status == OrderStatus.Draft)
                ?? throw new ValidationException("Cannot find Draft Status");

            // Create Order
            var order = new ORD_Order
            {
                UPL_Project = project,
                ORD_Status = draftStatus,
                StakeHolderEmail = project.StakeHolderEmail
            };

            // Save
            DB.ORD_Order.Add(order);
            await DB.SaveChangesAsync();

            return order.FormID;
        }

        public async Task CreateOrderLineAsync(OrderCreateLineDTO orderCreateLineDTO)
        {
            // Place Order on Hold
            await PlaceOrderOnHoldAsync(orderCreateLineDTO.OrderID);

            // Get Order making sure it's not shipped
            var order = await DB.ORD_Order
                .Include(x => x.ORD_Status)
                .FirstOrDefaultAsync(x => x.FormID == orderCreateLineDTO.OrderID
                    && x.ORD_Status.Status != OrderStatus.Shipped)
                ?? throw new ValidationException("Order not found or shipped.");

            // Get Part
            var part = await DB.UPL_PartInfo
                .FirstOrDefaultAsync(x => x.FormID == orderCreateLineDTO.PartID)
                ?? throw new ValidationException("Part not found.");

            // Make sure the part add doesn't exceed 50
            if (await DB.ORD_Line.CountAsync(x => x.ORD_Order.FormID == order.FormID) >= 50)
                throw new ValidationException("Order can't have more than 50 parts.");

            // If the Order isn't draft update to PickReady so it can be picked
            if (order.ORD_Status.Status != OrderStatus.Draft)
            {
                var pickReadyStatus = await DB.ORD_Status
                    .FirstOrDefaultAsync(x => x.Status == OrderStatus.PickReady)
                    ?? throw new ValidationException("Cannot find Pick Ready Status");

                order.ORD_Status = pickReadyStatus;
            }

            // Create Order Line
            order.ORD_Line.Add(new ORD_Line
            {
                UPL_PartInfo = part,
                Quantity = 1
            });

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task<OrderHeaderDTO> GetOrderHeaderAsync(Guid orderID)
        {
            // Place Order on Hold
            await PlaceOrderOnHoldAsync(orderID);

            // Get Order By OrderID
            return await DB.ORD_Order
                .Include(x => x.UPL_Project)
                    .ThenInclude(x => x.UPL_Customer)
                .Include(x => x.ORD_Status)
                .Where(x => x.FormID == orderID)
                .AsNoTracking()
                .Select(x => new OrderHeaderDTO
                {
                    FormID = x.FormID,
                    ProjectID = x.UPL_Project.FormID,
                    CustomerCode = x.UPL_Project.UPL_Customer.CustomerCode,
                    ProjectCode = x.UPL_Project.ProjectCode,
                    OrderStatus = x.ORD_Status.Status,
                    OrderID = x.Id,
                    RequestedShipDate = x.RequestedShipDate,
                    RequestedDeliveryDate = x.RequestedDeliveryDate,
                    ActualShipDate = x.ActualShipDate,
                    StakeHolderEmail = x.StakeHolderEmail,
                    OrderUDFLabel1 = x.UPL_Project.OrderUDF1,
                    OrderUDFLabel2 = x.UPL_Project.OrderUDF2,
                    OrderUDFLabel3 = x.UPL_Project.OrderUDF3,
                    OrderUDFLabel4 = x.UPL_Project.OrderUDF4,
                    OrderUDFLabel5 = x.UPL_Project.OrderUDF5,
                    OrderUDFLabel6 = x.UPL_Project.OrderUDF6,
                    OrderUDFLabel7 = x.UPL_Project.OrderUDF7,
                    OrderUDFLabel8 = x.UPL_Project.OrderUDF8,
                    OrderUDFLabel9 = x.UPL_Project.OrderUDF9,
                    OrderUDFLabel10 = x.UPL_Project.OrderUDF10,
                    OrderUDF1 = x.OrderUDF1,
                    OrderUDF2 = x.OrderUDF2,
                    OrderUDF3 = x.OrderUDF3,
                    OrderUDF4 = x.OrderUDF4,
                    OrderUDF5 = x.OrderUDF5,
                    OrderUDF6 = x.OrderUDF6,
                    OrderUDF7 = x.OrderUDF7,
                    OrderUDF8 = x.OrderUDF8,
                    OrderUDF9 = x.OrderUDF9,
                    OrderUDF10 = x.OrderUDF10,
                    IsClosed = x.ORD_Status.Status == OrderStatus.Shipped
                })
                .FirstOrDefaultAsync()
                ?? throw new ValidationException("Order not found.");
        }

        public async Task<List<OrderPartListDTO>> GetOrderLineAsync(Guid orderID)
        {
            // Place Order on Hold
            await PlaceOrderOnHoldAsync(orderID);

            var returnList = await DB.ORD_Line
                .Include(x => x.UPL_PartInfo)
                .Include(x => x.PO_Header)
                .Include(x => x.UPL_StockGroup)
                .Include(x => x.ORD_PickList)
                    .ThenInclude(x => x.INV_Stock)
                .AsSplitQuery()
                .Where(x => x.ORD_Order.FormID == orderID)
                .AsNoTracking()
                .Select(x => new OrderPartListDTO
                {
                    FormID = x.FormID,
                    PartID = x.UPL_PartInfo.FormID,
                    POID = x.PO_Header != null ? x.PO_Header.FormID : null,
                    StockGroupID = x.UPL_StockGroup != null ? x.UPL_StockGroup.FormID : null,
                    PartNumber = x.UPL_PartInfo.PartNumber,
                    PartDescription = x.UPL_PartInfo.PartDescription,
                    PO = x.PO_Header != null ? x.PO_Header.PONumber : null,
                    StockGroup = x.UPL_StockGroup != null ? x.UPL_StockGroup.StockGroupTitle : null,
                    SerialNumber = x.SerialNumber,
                    Quantity = x.Quantity,
                    PickedQuantity = x.ORD_PickList.Sum(i => i.INV_Stock.Quantity),

                    CommittedQuantity = DB.ORD_Line
                    .Where(y => y.OrderID != x.OrderID
                        && y.PartID == x.PartID
                        && y.ORD_Order.ProjectID == x.ORD_Order.ProjectID
                        && (y.ORD_Order.ORD_Status.Status == OrderStatus.PickReady || y.ORD_Order.ORD_Status.Status == OrderStatus.Picking)
                        && (!x.POHeaderID.HasValue || y.POHeaderID == x.POHeaderID)
                        && (!x.StockGroupID.HasValue || y.StockGroupID == x.StockGroupID)
                        && (x.SerialNumber == null || y.SerialNumber == x.SerialNumber))
                    .Select(y => y.Quantity - y.ORD_PickList.Sum(p => (int?)p.INV_Stock.Quantity) ?? 0)
                    .Sum(),

                    InStockQuantity = DB.INV_Stock
                    .Where(y =>
                        y.PO_Line.PartID == x.PartID
                        && y.PO_Line.PO_Header.ProjectID == x.ORD_Order.ProjectID
                        && y.INV_StockStatus.StockStatus == StockStatus.InStock &&
                        (!x.POHeaderID.HasValue || y.PO_Line.POHeaderID == x.POHeaderID) &&
                        (!x.StockGroupID.HasValue || y.StockGroupID == x.StockGroupID) &&
                        (x.SerialNumber == null || y.SerialNumber == x.SerialNumber))
                    .Sum(s => s.Quantity)
                })
                .ToListAsync();

            return returnList;
        }

        public async Task<PagedTable<OrderSearchReturnDTO>> SearchOrderAsync(SearchQueryDTO searchQuery)
        {
            // Build base Query
            var query = DB.ORD_Order
                .Include(x => x.UPL_Project)
                .Include(x => x.ORD_Status)
                .AsQueryable();

            // Filters
            if (!string.IsNullOrEmpty(searchQuery.SearchFilter))
            {
                var searchFilter = searchQuery.SearchFilter
                    .Split(',')
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

                foreach (var filter in searchFilter)
                {
                    query = query.Where(x =>
                        x.Id.ToString().Contains(filter) ||
                        x.UPL_Project.ProjectCode.Contains(filter) ||
                        x.ORD_Status.Status.Contains(filter) ||
                        x.RequestedShipDate.ToString().Contains(filter) ||
                        x.RequestedDeliveryDate.ToString().Contains(filter) ||
                        x.ActualShipDate.ToString().Contains(filter)
                    );
                }
            }

            // Apply Order By
            switch (searchQuery.SortColumn)
            {
                case 1:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.Id)
                        : query.OrderByDescending(x => x.Id);
                    break;
                case 2:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.UPL_Project.ProjectCode)
                        : query.OrderByDescending(x => x.UPL_Project.ProjectCode);
                    break;
                case 3:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.ORD_Status.Status)
                        : query.OrderByDescending(x => x.ORD_Status.Status);
                    break;
                case 4:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.RequestedShipDate)
                        : query.OrderByDescending(x => x.RequestedShipDate);
                    break;
                case 5:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.RequestedDeliveryDate)
                        : query.OrderByDescending(x => x.RequestedDeliveryDate);
                    break;
                case 6:
                    query = searchQuery.SortOrder == 1
                        ? query.OrderBy(x => x.ActualShipDate)
                        : query.OrderByDescending(x => x.ActualShipDate);
                    break;
                default:
                    query = query.OrderBy(x => x.Id);
                    break;
            }

            // Apply pagination and projection
            var orderList = await query
                .Skip(searchQuery.RecordSize * (searchQuery.Page - 1))
                .Take(searchQuery.RecordSize)
                .AsNoTracking()
                .Select(x => new OrderSearchReturnDTO
                {
                    FormID = x.FormID,
                    OrderID = x.Id,
                    ProjectCode = x.UPL_Project.ProjectCode,
                    OrderStatus = x.ORD_Status.Status,
                    RequestedShipDate = x.RequestedShipDate,
                    RequestedDeliveryDate = x.RequestedDeliveryDate,
                    ActualShipDate = x.ActualShipDate
                })
                .ToListAsync();

            // Return Object
            return new PagedTable<OrderSearchReturnDTO>
            {
                Data = orderList,
                TotalRecords = await query.CountAsync(),
                PageIndex = searchQuery.Page
            };
        }

        public async Task UpdateOrderHeaderAsync(OrderHeaderUpdateDTO orderHeaderUpdateDTO)
        {
            // Place Order on Hold
            await PlaceOrderOnHoldAsync(orderHeaderUpdateDTO.FormID);

            // Get Order making sure it's not shipped
            var order = await DB.ORD_Order
                .FirstOrDefaultAsync(x => x.FormID == orderHeaderUpdateDTO.FormID
                    && x.ORD_Status.Status != OrderStatus.Shipped)
                ?? throw new ValidationException("Order not found or shipped.");

            // Update Order
            order.RequestedShipDate = orderHeaderUpdateDTO.RequestedShipDate;
            order.RequestedDeliveryDate = orderHeaderUpdateDTO.RequestedDeliveryDate;
            order.ActualShipDate = orderHeaderUpdateDTO.ActualShipDate;
            order.StakeHolderEmail = orderHeaderUpdateDTO.StakeHolderEmail;
            order.OrderUDF1 = orderHeaderUpdateDTO.OrderUDF1;
            order.OrderUDF2 = orderHeaderUpdateDTO.OrderUDF2;
            order.OrderUDF3 = orderHeaderUpdateDTO.OrderUDF3;
            order.OrderUDF4 = orderHeaderUpdateDTO.OrderUDF4;
            order.OrderUDF5 = orderHeaderUpdateDTO.OrderUDF5;
            order.OrderUDF6 = orderHeaderUpdateDTO.OrderUDF6;
            order.OrderUDF7 = orderHeaderUpdateDTO.OrderUDF7;
            order.OrderUDF8 = orderHeaderUpdateDTO.OrderUDF8;
            order.OrderUDF9 = orderHeaderUpdateDTO.OrderUDF9;
            order.OrderUDF10 = orderHeaderUpdateDTO.OrderUDF10;

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task DeleteOrderLineAsync(Guid lineID)
        {
            // Get order line checking if order isn't shipped
            var orderLine = await DB.ORD_Line
                .Include(x => x.ORD_PickList)
                    .ThenInclude(x => x.INV_Stock)
                .Include(x => x.ORD_Order)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.FormID == lineID
                    && x.ORD_Order.ORD_Status.Status != OrderStatus.Shipped)
                ?? throw new ValidationException("Order Line not found or Order is shipped.");

            // Place Order on Hold
            await PlaceOrderOnHoldAsync(orderLine.ORD_Order.FormID);

            // Soft Delete Order Line
            orderLine.IsDelete = true;

            // Get In Stock Stock Status
            var inStockStatus = await DB.INV_StockStatus
                .FirstOrDefaultAsync(x => x.StockStatus == StockStatus.InStock)
                ?? throw new ValidationException("Cannot find In Stock Status");

            // Get any stock picked to the order line
            foreach (var line in orderLine.ORD_PickList)
            {
                // Soft Delete Pick List
                line.IsDelete = true;

                // Update Stock Back to In Stock
                line.INV_Stock.INV_StockStatus = inStockStatus;
            }

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task UpdateOrderLineAsync(OrderPartListUpdate orderPartListUpdateDTO)
        {
            // Get order line checking if order isn't shipped
            var orderLine = await DB.ORD_Line
                .Include(x => x.ORD_PickList)
                    .ThenInclude(x => x.INV_Stock)
                .Include(x => x.ORD_Order)
                .FirstOrDefaultAsync(x => x.FormID == orderPartListUpdateDTO.FormID
                    && x.ORD_Order.ORD_Status.Status != OrderStatus.Shipped)
                ?? throw new ValidationException("Order Line not found or Order is shipped.");

            // Place Order on Hold
            await PlaceOrderOnHoldAsync(orderLine.ORD_Order.FormID);

            // Check if QTY > 0
            if (orderPartListUpdateDTO.Quantity <= 0)
                throw new ValidationException("Quantity must be greater than 0.");

            // Check if QTY < Picked Quantity
            if (orderPartListUpdateDTO.Quantity < orderLine.ORD_PickList.Sum(x => (int?)x.INV_Stock.Quantity ?? 0))
                throw new ValidationException("Quantity cannot be less than the picked quantity.");

            // Get PO 
            if (orderPartListUpdateDTO.POID.HasValue)
            {
                var po = await DB.PO_Header
                    .FirstOrDefaultAsync(x => x.FormID == orderPartListUpdateDTO.POID)
                    ?? throw new ValidationException("PO not found.");

                orderLine.PO_Header = po;
            }
            else
                orderLine.POHeaderID = null;

            // Get Stock Group
            if (orderPartListUpdateDTO.StockGroupID.HasValue)
            {
                var stockGroup = await DB.UPL_StockGroup
                    .FirstOrDefaultAsync(x => x.FormID == orderPartListUpdateDTO.StockGroupID)
                    ?? throw new ValidationException("Stock Group not found.");

                orderLine.UPL_StockGroup = stockGroup;
            }
            else
                orderLine.StockGroupID = null;

            // Update Serial Number
            orderLine.SerialNumber = orderPartListUpdateDTO.SerialNumber;

            // Update Quantity
            orderLine.Quantity = orderPartListUpdateDTO.Quantity;

            // Save
            await DB.SaveChangesAsync();

        }

        public async Task<OrderShipDTO> GetOrderShippingAsync(Guid orderID)
        {
            // Place Order on Hold
            await PlaceOrderOnHoldAsync(orderID);

            // Get Order By OrderID
            return await DB.ORD_Order
                .Include(x => x.UPL_Project)
                    .ThenInclude(x => x.UPL_Customer)
                .Include(x => x.ORD_Status)
                .Where(x => x.FormID == orderID)
                .AsNoTracking()
                .Select(x => new OrderShipDTO
                {
                    FormID = x.FormID,
                    ProjectID = x.UPL_Project.FormID,
                    CustomerCode = x.UPL_Project.UPL_Customer.CustomerCode,
                    ProjectCode = x.UPL_Project.ProjectCode,
                    OrderStatus = x.ORD_Status.Status,
                    OrderID = x.Id,
                    Address1 = x.Address1,
                    Address2 = x.Address2,
                    City = x.City,
                    State = x.State,
                    Zip = x.Zip,
                    ShipContact = x.ShipContact,
                    ShipPhone = x.ShipPhone,
                    ShipEmail = x.ShipEmail,
                    Carrier = x.Carrier
                })
                .FirstOrDefaultAsync()
                ?? throw new ValidationException("Order not found.");
        }
    }
}