using Microsoft.EntityFrameworkCore;
using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Inventory;
using OrderTrak.API.Models.DTO.Order;
using OrderTrak.API.Models.OrderTrakDB;
using OrderTrak.API.Services.Inventory;
using OrderTrak.API.Static;
using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Services.Order
{
    public class OrderService(OrderTrakContext orderTrakContext, IInventoryService inventoryService) : IOrderService
    {
        private readonly OrderTrakContext DB = orderTrakContext;

        private readonly IInventoryService InventoryService = inventoryService;

        private async Task PlaceOrderOnHoldAsync(Guid orderID)
        {
            // Get Order making sure it's not shipped, draft, Cancel or already on hold
            var order = await DB.ORD_Order
                .Include(x => x.ORD_Status)
                .FirstOrDefaultAsync(x => x.FormID == orderID
                    && x.ORD_Status.Status != OrderStatus.Shipped
                    && x.ORD_Status.Status != OrderStatus.Draft
                    && x.ORD_Status.Status != OrderStatus.Hold
                    && x.ORD_Status.Status != OrderStatus.Cancel
                    );

            if (order != null)
            {
                // Set Order Status Before Hold
                order.ORD_StatusBeforeHold = order.ORD_Status;

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
            // Get Order By OrderID
            return await DB.ORD_Order
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
                    IsClosed = x.ORD_Status.Status == OrderStatus.Shipped || x.ORD_Status.Status == OrderStatus.Cancel,
                    IsCanceled = x.ORD_Status.Status == OrderStatus.Cancel
                })
                .FirstOrDefaultAsync()
                ?? throw new ValidationException("Order not found.");
        }

        public async Task<List<OrderPartListDTO>> GetOrderLineAsync(OrderPartListSearchDTO orderPartListSearchDTO)
        {

            var orderLineQuery = DB.ORD_Line
                .AsSplitQuery()
                .Where(x => x.ORD_Order.FormID == orderPartListSearchDTO.FormID)
                .AsNoTracking();

            // Stock Only Filter
            if (orderPartListSearchDTO.StockOnly)
                orderLineQuery = orderLineQuery.Where(x => x.UPL_PartInfo.IsStock);

            return await orderLineQuery
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
                        && (!y.POHeaderID.HasValue || y.POHeaderID == x.POHeaderID)
                        && (!y.StockGroupID.HasValue || y.StockGroupID == x.StockGroupID)
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
                    .Sum(s => s.Quantity),
                    IsStock = x.UPL_PartInfo.IsStock
                })
                .ToListAsync();
        }

        public async Task<PagedTable<OrderSearchReturnDTO>> SearchOrderAsync(OrderSearchDTO searchQuery)
        {
            // Build base Query
            var query = DB.ORD_Order
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

            // Cancel Filter
            if (!searchQuery.ShowCancel)
                query = query.Where(x => x.ORD_Status.Status != OrderStatus.Cancel);

            // Ship Filter
            if (!searchQuery.ShowShipped)
                query = query.Where(x => x.ORD_Status.Status != OrderStatus.Shipped);

            // Picking Only Filter
            if (searchQuery.PickingOnly)
                query = query.Where(x => x.ORD_Status.Status == OrderStatus.PickReady || x.ORD_Status.Status == OrderStatus.Picking);

            // Ship Ready Only Filter
            if (searchQuery.ShipReadyOnly)
                query = query.Where(x => x.ORD_Status.Status == OrderStatus.Picked || x.ORD_Status.Status == OrderStatus.Shipped);

            // Apply Order By
            query = searchQuery.SortColumn switch
            {
                1 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.Id)
                                        : query.OrderByDescending(x => x.Id),
                2 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.UPL_Project.ProjectCode)
                                        : query.OrderByDescending(x => x.UPL_Project.ProjectCode),
                3 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.ORD_Status.Status)
                                        : query.OrderByDescending(x => x.ORD_Status.Status),
                4 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.RequestedShipDate)
                                        : query.OrderByDescending(x => x.RequestedShipDate),
                5 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.RequestedDeliveryDate)
                                        : query.OrderByDescending(x => x.RequestedDeliveryDate),
                6 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.ActualShipDate)
                                        : query.OrderByDescending(x => x.ActualShipDate),
                _ => query.OrderBy(x => x.Id),
            };

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

            // Get Order
            var order = await DB.ORD_Order
                .Include(x => x.ORD_Status)
                .FirstOrDefaultAsync(x => x.FormID == orderHeaderUpdateDTO.FormID)
                ?? throw new ValidationException("Order not found.");

            // Update Order Header
            order.RequestedShipDate = orderHeaderUpdateDTO.RequestedShipDate;
            order.RequestedDeliveryDate = orderHeaderUpdateDTO.RequestedDeliveryDate;
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
            if (string.IsNullOrEmpty(orderPartListUpdateDTO.SerialNumber))
                orderLine.SerialNumber = null;
            else
                orderLine.SerialNumber = orderPartListUpdateDTO.SerialNumber;

            // Update Quantity
            orderLine.Quantity = orderPartListUpdateDTO.Quantity;

            // Save
            await DB.SaveChangesAsync();

        }

        public async Task<OrderShipDTO> GetOrderShippingAsync(Guid orderID)
        {
            // Get Order By OrderID
            return await DB.ORD_Order
                .Where(x => x.FormID == orderID)
                .AsNoTracking()
                .Select(x => new OrderShipDTO
                {
                    FormID = x.FormID,
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

        public async Task UpdateOrderShippingAsync(OrderShipUpdateDTO orderShipUpdateDTO)
        {
            // Place Order on Hold
            await PlaceOrderOnHoldAsync(orderShipUpdateDTO.FormID);

            // Get Order
            var order = await DB.ORD_Order
                .Include(x => x.ORD_Status)
                .FirstOrDefaultAsync(x => x.FormID == orderShipUpdateDTO.FormID
                    && x.ORD_Status.Status != OrderStatus.Shipped)
                ?? throw new ValidationException("Order not found or it is shipped.");

            // Update Ship Info
            order.Address1 = orderShipUpdateDTO.Address1;
            order.Address2 = orderShipUpdateDTO.Address2;
            order.City = orderShipUpdateDTO.City;
            order.State = orderShipUpdateDTO.State;
            order.Zip = orderShipUpdateDTO.Zip;
            order.ShipContact = orderShipUpdateDTO.ShipContact;
            order.ShipPhone = orderShipUpdateDTO.ShipPhone;
            order.ShipEmail = orderShipUpdateDTO.ShipEmail;
            order.Carrier = orderShipUpdateDTO.Carrier;

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task<OrderActivationDTO> GetOrderActivationAsync(Guid orderID)
        {
            // Get Order By OrderID
            return await DB.ORD_Order
                .Where(x => x.FormID == orderID)
                .AsNoTracking()
                .Select(x => new OrderActivationDTO
                {
                    FormID = x.FormID,
                    StatusID = x.ORD_StatusBeforeHold != null ? x.ORD_StatusBeforeHold.FormID : null,
                    OrderNote = x.OrderNote
                })
                .FirstOrDefaultAsync()
                ?? throw new ValidationException("Order not found.");
        }

        public async Task UpdateOrderActivationAsync(OrderActivationUpdateDTO orderActivationUpdateDTO)
        {
            // Place Order on Hold
            await PlaceOrderOnHoldAsync(orderActivationUpdateDTO.FormID);

            // Get Order
            var order = await DB.ORD_Order
                .Include(x => x.ORD_Status)
                .FirstOrDefaultAsync(x => x.FormID == orderActivationUpdateDTO.FormID
                    && x.ORD_Status.Status != OrderStatus.Shipped)
                ?? throw new ValidationException("Order not found or it is shipped.");

            // If draft set update status to PickReady
            var updateStatus = order.ORD_Status.Status == OrderStatus.Draft
                    ? await DB.ORD_Status.FirstOrDefaultAsync(x => x.Status == OrderStatus.PickReady)
                        ?? throw new ValidationException("Cannot find Pick Ready Status.")
                    : await DB.ORD_Status.FirstOrDefaultAsync(x => x.FormID == orderActivationUpdateDTO.StatusID)
                        ?? throw new ValidationException("Cannot find Status.");

            // Error Check RequestedShipDate has to be greater than equal to today and exist
            if (!order.RequestedShipDate.HasValue || order.RequestedShipDate.Value.Date < DateTime.UtcNow.Date)
                throw new ValidationException("Requested date required and cannot be in the past.");

            // Error Check RequestedDeliveryDate has to exist and be greater than or equal to RequestedShipDate
            if (!order.RequestedDeliveryDate.HasValue || order.RequestedDeliveryDate.Value.Date < order.RequestedShipDate.Value.Date)
                throw new ValidationException("Requested delivery date required and cannot be before requested ship date.");

            // Error Check Address1
            if (string.IsNullOrEmpty(order.Address1))
                throw new ValidationException("Address1 is required.");

            // Error Check City
            if (string.IsNullOrEmpty(order.City))
                throw new ValidationException("City is required.");

            // Error Check State
            if (string.IsNullOrEmpty(order.State))
                throw new ValidationException("State is required.");

            // Error Check Zip
            if (string.IsNullOrEmpty(order.Zip))
                throw new ValidationException("Zip is required.");

            // Error Check ShipContact
            if (string.IsNullOrEmpty(order.ShipContact))
                throw new ValidationException("Ship Contact is required.");

            // Error Check ShipPhone
            if (string.IsNullOrEmpty(order.ShipPhone))
                throw new ValidationException("Ship Phone is required.");

            // Error Check ShipEmail
            if (string.IsNullOrEmpty(order.ShipEmail))
                throw new ValidationException("Ship Email is required.");

            // Error Check Carrier
            if (string.IsNullOrEmpty(order.Carrier))
                throw new ValidationException("Carrier is required.");

            // Get Order Lines
            var orderLines = await GetOrderLineAsync(new OrderPartListSearchDTO { FormID = order.FormID, StockOnly = true });

            // Check if Order Lines are empty
            if (orderLines.Count == 0)
                throw new ValidationException("Order must have at least one stock line.");

            // Check if Order Lines have enough qty
            foreach (var line in orderLines)
            {
                if (line.PickedQuantity > line.Quantity)
                    throw new ValidationException($"Order Line {line.PartNumber} has invalid picked quantity.");

                var neededQty = line.Quantity - line.PickedQuantity;

                // Check if Order Line has enough qty
                if ((line.InStockQuantity - line.CommittedQuantity) < neededQty)
                    throw new ValidationException($"Order Line {line.PartNumber} does not have enough qty. {neededQty} needed.");
            }

            // Update Order
            order.ORD_Status = updateStatus;
            order.OrderNote = orderActivationUpdateDTO.OrderNote;

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task CancelOrderAsync(OrderCancelDTO orderCancelDTO)
        {
            // Place Order on Hold
            await PlaceOrderOnHoldAsync(orderCancelDTO.FormID);

            // Get Order
            var order = await DB.ORD_Order
                .Include(x => x.ORD_Status)
                .FirstOrDefaultAsync(x => x.FormID == orderCancelDTO.FormID
                    && x.ORD_Status.Status != OrderStatus.Shipped)
                ?? throw new ValidationException("Order not found or it is shipped.");

            // Get Cancel Status
            var cancelStatus = await DB.ORD_Status
                .FirstOrDefaultAsync(x => x.Status == OrderStatus.Cancel)
                ?? throw new ValidationException("Cannot find Cancel Status");

            // Get Order Lines
            var orderLines = await GetOrderLineAsync(new OrderPartListSearchDTO { FormID = order.FormID });

            // Remove Order Lines
            foreach (var line in orderLines)
                await DeleteOrderLineAsync(line.FormID);

            // Update Order to Cancel
            order.ORD_Status = cancelStatus;

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task PickToOrderAsync(OrderPickDTO orderPickDTO)
        {

            // Get Order Line
            var orderLine = await DB.ORD_Line
                .Include(x => x.ORD_Order)
                    .ThenInclude(x => x.ORD_Status)
                .Include(x => x.ORD_PickList)
                    .ThenInclude(x => x.INV_Stock)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.FormID == orderPickDTO.OrderLineID
                    && (x.ORD_Order.ORD_Status.Status == OrderStatus.PickReady || x.ORD_Order.ORD_Status.Status == OrderStatus.Picking))
                ?? throw new ValidationException("Order Line not found or the order is not in a pickable state.");

            // Make Sure the line isn't already picked in full
            var pickedQTY = orderLine.ORD_PickList.Sum(x => x.INV_Stock.Quantity);

            if (pickedQTY >= orderLine.Quantity)
                throw new ValidationException("Order Line already picked in full.");

            // Get Inventory
            var inventory = await InventoryService.SearchInventoryAsync(new InventorySearchDTO
            {
                OrderLineID = orderPickDTO.OrderLineID,
                InventoryID = orderPickDTO.InventoryID
            });

            if (inventory.Data.Count == 0)
                throw new ValidationException("Inventory not found.");

            var inv = inventory.Data.First();

            // Get On Order Inventory Status
            var onOrderStatusTask = DB.INV_StockStatus
                .FirstOrDefaultAsync(x => x.StockStatus == StockStatus.OnOrder);

            // Get Picking Order Status
            var pickingStatusTask = DB.ORD_Status
                .FirstOrDefaultAsync(x => x.Status == OrderStatus.Picking);
                
            await Task.WhenAll(onOrderStatusTask, pickingStatusTask);

            var onOrderStatus = onOrderStatusTask.Result
                ?? throw new ValidationException("Cannot find On Order Status");

            var pickingStatus = pickingStatusTask.Result
                ?? throw new ValidationException("Cannot find Picking Status");

            // Get Inventory
            var pickedStock = await DB.INV_Stock
                .FirstOrDefaultAsync(x => x.FormID == inv.FormID)
                ?? throw new ValidationException("Inventory not found.");

            // If Inventory will exceed the order line quantity split and select new box
            if (pickedStock.Quantity + pickedQTY > orderLine.Quantity)
            {
                // Split Box
                var splitBoxID = await InventoryService.SplitBoxIDAsync(pickedStock.FormID, orderLine.Quantity - pickedQTY);

                // Get New Box
                pickedStock = await DB.INV_Stock
                    .FirstOrDefaultAsync(x => x.FormID == splitBoxID)
                    ?? throw new ValidationException("Inventory not found.");
            }

            // Create Pick List
            var pickList = new ORD_PickList
            {
                ORD_Line = orderLine,
                INV_Stock = pickedStock
            };

            // Add to Order Line
            orderLine.ORD_PickList.Add(pickList);

            // Update Stock Status to On Order
            pickList.INV_Stock.INV_StockStatus = onOrderStatus;


            orderLine.ORD_Order.ORD_Status = pickingStatus;

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task<bool> IsDonePickAsync(OrderPickDoneDTO orderPickDoneDTO)
        {
            // Get Order Lines
            var orderLines = await GetOrderLineAsync(new OrderPartListSearchDTO { FormID = orderPickDoneDTO.OrderID, StockOnly = true });

            if (!orderLines.All(x => x.Quantity == x.PickedQuantity))
                return false;

            // Get Order
            var order = await DB.ORD_Order
                .FirstOrDefaultAsync(x => x.FormID == orderPickDoneDTO.OrderID
                    && (x.ORD_Status.Status == OrderStatus.PickReady || x.ORD_Status.Status == OrderStatus.Picking));

            if (order != null)
            {
                // Get Picked Status
                var pickedStatus = await DB.ORD_Status
                    .FirstOrDefaultAsync(x => x.Status == OrderStatus.Picked)
                    ?? throw new ValidationException("Cannot find Picked Status");

                // Update Order
                order.ORD_Status = pickedStatus;

                // Save
                await DB.SaveChangesAsync();
            }

            return true;
        }

        public async Task RemovePickFromOrderAsync(OrderPickRemoveDTO orderPickRemoveDTO)
        {
            // Get order line checking if order isn't shipped
            var pickedConnection = await DB.ORD_PickList
                .Include(x => x.INV_Stock)
                .Include(x => x.ORD_Line)
                    .ThenInclude(x => x.ORD_Order)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.INV_Stock.FormID == orderPickRemoveDTO.InventoryID
                    && x.ORD_Line.FormID == orderPickRemoveDTO.OrderLineID
                    && x.ORD_Line.ORD_Order.ORD_Status.Status != OrderStatus.Shipped)
                ?? throw new ValidationException("Pick not found or order has already been shipped.");

            // Place Order on Hold
            await PlaceOrderOnHoldAsync(pickedConnection.ORD_Line.ORD_Order.FormID);

            // Get In Stock Stock Status
            var inStockStatus = await DB.INV_StockStatus
                .FirstOrDefaultAsync(x => x.StockStatus == StockStatus.InStock)
                ?? throw new ValidationException("Cannot find In Stock Status");

            // Soft Delete Pick List
            pickedConnection.IsDelete = true;

            // Update Stock Back to In Stock
            pickedConnection.INV_Stock.INV_StockStatus = inStockStatus;
            
            // Save
            await DB.SaveChangesAsync();
        }

        public async Task CreateTrackingForOrderAsync(OrderCreateTrackingDTO orderCreateTrackingDTO)
        {
            // Get Order
            var order = await DB.ORD_Order
                .Include(x => x.ORD_Tracking)
                .FirstOrDefaultAsync(x => x.FormID == orderCreateTrackingDTO.FormID
                    && x.ORD_Status.Status != OrderStatus.Shipped)
                ?? throw new ValidationException("Order not found or it is shipped.");

            // Check for Invalid Box Count
            if (orderCreateTrackingDTO.BoxCount <= 0)
                throw new ValidationException("Box Count must be greater than 0.");

            // Check for Invalid Weight
            if (orderCreateTrackingDTO.Weight <= 0)
                throw new ValidationException("Weight must be greater than 0.");

            // Check for Invalid Pallet Count
            if (orderCreateTrackingDTO.PalletCount.HasValue && orderCreateTrackingDTO.PalletCount < 0)
                throw new ValidationException("Pallet Count must be greater than or equal to 0.");

            // Check for Duplicates
            if (order.ORD_Tracking.Any(x => x.Tracking == orderCreateTrackingDTO.TrackingNumber))
                throw new ValidationException("Tracking number already exists.");

            // Create Tracking
            order.ORD_Tracking.Add(new ORD_Tracking
            {
                Tracking = orderCreateTrackingDTO.TrackingNumber,
                BoxCount = orderCreateTrackingDTO.BoxCount,
                Weight = orderCreateTrackingDTO.Weight,
                PalletCount = orderCreateTrackingDTO.PalletCount
            });

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task<PagedTable<OrderTrackingSearchReturnDTO>> SearchOrderTrackingAsync(OrderTrackingSearchDTO searchQuery)
        {
            // Build base Query
            var query = DB.ORD_Tracking
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
                        x.Tracking.Contains(filter) ||
                        x.BoxCount.ToString().Contains(filter) ||
                        x.Weight.ToString().Contains(filter) ||
                        x.PalletCount.ToString().Contains(filter)
                    );
                }
            }

            // Apply Order Filter
            if (searchQuery.OrderID.HasValue)
                query = query.Where(x => x.ORD_Order.FormID == searchQuery.OrderID);

            // Apply Order By
            query = searchQuery.SortColumn switch
            {
                1 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.Tracking)
                                        : query.OrderByDescending(x => x.Tracking),
                2 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.BoxCount)
                                        : query.OrderByDescending(x => x.BoxCount),
                3 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.Weight)
                                        : query.OrderByDescending(x => x.Weight),
                4 => searchQuery.SortOrder == 1
                                        ? query.OrderBy(x => x.PalletCount)
                                        : query.OrderByDescending(x => x.PalletCount),
                _ => query.OrderBy(x => x.Tracking),
            };

            // Apply pagination and projection
            var trackingList = await query
                .Skip(searchQuery.RecordSize * (searchQuery.Page - 1))
                .Take(searchQuery.RecordSize)
                .AsNoTracking()
                .Select(x => new OrderTrackingSearchReturnDTO
                {
                    FormID = x.FormID,
                    Tracking = x.Tracking,
                    BoxCount = x.BoxCount,
                    Weight = x.Weight,
                    PalletCount = x.PalletCount
                })
                .ToListAsync();

            // Return Object
            return new PagedTable<OrderTrackingSearchReturnDTO>
            {
                Data = trackingList,
                TotalRecords = await query.CountAsync(),
                PageIndex = searchQuery.Page
            };
        }

        public async Task DeleteTrackingFromOrderAsync(Guid trackingID)
        {
            // Get Order Tracking
            var orderTracking = await DB.ORD_Tracking
                .FirstOrDefaultAsync(x => x.FormID == trackingID
                    && x.ORD_Order.ORD_Status.Status != OrderStatus.Shipped)
                ?? throw new ValidationException("Order Tracking not found or Order is shipped.");

            // Soft Delete Tracking
            orderTracking.IsDelete = true;

            // Save
            await DB.SaveChangesAsync();
        }

        public async Task CompleteShippingOrderAsync(OrderCompleteShippingDTO orderCompleteShippingDTO)
        {
            // Get Order
            var order = await DB.ORD_Order
                .Include(x => x.ORD_Tracking)
                .Include(x => x.ORD_Line)
                    .ThenInclude(x => x.ORD_PickList)
                        .ThenInclude(x => x.INV_Stock)
                            .ThenInclude(x => x.INV_StockStatus)
                .Include(x => x.ORD_Status)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.FormID == orderCompleteShippingDTO.OrderID
                    && x.ORD_Status.Status != OrderStatus.Shipped)
                ?? throw new ValidationException("Order not found or it is shipped.");

            // Get required statuses
            var shippedStatusTask = DB.ORD_Status
                .FirstOrDefaultAsync(x => x.Status == OrderStatus.Shipped);

            var shippedStockStatusTask = DB.INV_StockStatus
                .FirstOrDefaultAsync(x => x.StockStatus == StockStatus.Shipped);

            await Task.WhenAll(shippedStatusTask, shippedStockStatusTask);

            var shippedStatus = shippedStatusTask.Result
                ?? throw new ValidationException("Shipped order status not found.");

            var shippedStockStatus = shippedStockStatusTask.Result
                ?? throw new ValidationException("Shipped stock status not found.");

            // Check if Order has Tracking
            if (order.ORD_Tracking.Count == 0)
                throw new ValidationException("Order must have at least one tracking number.");

            // Get Order Lines
            var orderLines = await GetOrderLineAsync(new OrderPartListSearchDTO { FormID = order.FormID, StockOnly = true });

            // Check if Picked Counts are equal to Order Line Counts
            if(orderLines.Any(x => x.Quantity != x.PickedQuantity))
                throw new ValidationException("Order Line picked quantity does not match order line quantity.");

            // Update all Inv_Stock to Shipped
            foreach (var inv in order.ORD_Line.SelectMany(x => x.ORD_PickList.Select(x => x.INV_Stock)))
                inv.INV_StockStatus = shippedStockStatus;

            // Update Order to Shipped
            order.ORD_Status = shippedStatus;
            order.ActualShipDate = DateTime.UtcNow;

            // Save
            await DB.SaveChangesAsync();
        }
    }
}