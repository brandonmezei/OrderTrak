using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Order
{
    public interface IOrderService
    {
        Task<Guid> CreateOrderAsync(OrderCreateDTO orderCreateDTO);
        Task UpdateOrderHeaderAsync(OrderHeaderUpdateDTO orderHeaderUpdateDTO);
        Task<OrderHeaderDTO> GetOrderHeaderAsync(Guid orderID);
        Task<PagedTableOfOrderSearchReturnDTO> SearchOrderAsync(OrderSearchDTO searchQuery);
        Task CreateOrderLineAsync(OrderCreateLineDTO orderCreateLineDTO);
        Task<List<OrderPartListDTO>> GetOrderLineAsync(OrderPartListSearchDTO orderPartListSearchDTO);
        Task DeleteOrderLineAsync(Guid lineID);
        Task UpdateOrderLineAsync(OrderPartListUpdate orderPartListUpdateDTO);
        Task<OrderShipDTO> GetOrderShippingAsync(Guid orderID);
        Task UpdateOrderShippingAsync(OrderShipUpdateDTO orderShipUpdateDTO);
        Task<OrderActivationDTO> GetOrderActivationAsync(Guid orderID);
        Task UpdateOrderActivationAsync(OrderActivationUpdateDTO orderActivationUpdateDTO);
        Task CancelOrderAsync(OrderCancelDTO orderCancelDTO);
        Task PickToOrderAsync(OrderPickDTO orderPickDTO);
    }
}
