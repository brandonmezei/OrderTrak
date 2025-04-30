using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
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
            // Get Order making sure it's not shipped
            var order = await DB.ORD_Order
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

            return await DB.ORD_Line
                .Include(x => x.UPL_PartInfo)
                .Include(x => x.PO_Line.PO_Header)
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
                      POLineID = x.PO_Line != null ? x.PO_Line.FormID : null,
                      StockGroupID = x.UPL_StockGroup != null ? x.UPL_StockGroup.FormID : null,
                      PartNumber = x.UPL_PartInfo.PartNumber,
                      PartDescription = x.UPL_PartInfo.PartDescription,
                      PO = x.PO_Line != null ? x.PO_Line.PO_Header.PONumber : null,
                      StockGroup = x.UPL_StockGroup != null ? x.UPL_StockGroup.StockGroupTitle : null,
                      Quantity = x.Quantity,
                      PickedQuantity = x.ORD_PickList.Sum(i => i.INV_Stock.Quantity)
                  })
                .ToListAsync();
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
    }
}