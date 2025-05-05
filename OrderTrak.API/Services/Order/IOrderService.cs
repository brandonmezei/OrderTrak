using OrderTrak.API.Models.DTO;
using OrderTrak.API.Models.DTO.Order;

namespace OrderTrak.API.Services.Order
{
    public interface IOrderService
    {
        Task<Guid> CreateOrderAsync(OrderCreateDTO orderCreateDTO);
        Task UpdateOrderHeaderAsync(OrderHeaderUpdateDTO orderHeaderUpdateDTO);
        Task<OrderHeaderDTO> GetOrderHeaderAsync(Guid orderID);
        Task<PagedTable<OrderSearchReturnDTO>> SearchOrderAsync(SearchQueryDTO searchQuery);
        Task CreateOrderLineAsync(OrderCreateLineDTO orderCreateLineDTO);
        Task<List<OrderPartListDTO>> GetOrderLineAsync(Guid orderID);
        Task DeleteOrderLineAsync(Guid lineID);
        Task UpdateOrderLineAsync(OrderPartListUpdate orderPartListUpdateDTO);
        Task<OrderShipDTO> GetOrderShippingAsync(Guid orderID);
    }
}