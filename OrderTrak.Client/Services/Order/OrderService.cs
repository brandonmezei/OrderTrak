using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Order
{
    public class OrderService(IClient client) : IOrderService
    {
        private readonly IClient ApiClient = client;

        public async Task<Guid> CreateOrderAsync(OrderCreateDTO orderCreateDTO)
        {
            return await ApiClient.CreateOrderAsync(orderCreateDTO);
        }

        public async Task CreateOrderLineAsync(OrderCreateLineDTO orderCreateLineDTO)
        {
            await ApiClient.CreateOrderLineAsync(orderCreateLineDTO);
        }

        public async Task DeleteOrderLineAsync(Guid lineID)
        {
            await ApiClient.DeleteOrderLineAsync(lineID);
        }

        public async Task<OrderHeaderDTO> GetOrderHeaderAsync(Guid orderID)
        {
            return await ApiClient.GetOrderHeaderAsync(orderID);
        }

        public async Task<List<OrderPartListDTO>> GetOrderLineAsync(Guid orderID)
        {
            return [.. await ApiClient.GetOrderLineAsync(orderID)];
        }

        public async Task<OrderShipDTO> GetOrderShippingAsync(Guid orderID)
        {
            return await ApiClient.GetOrderShippingAsync(orderID);
        }

        public async Task<PagedTableOfOrderSearchReturnDTO> SearchOrderAsync(SearchQueryDTO searchQuery)
        {
            return await ApiClient.SearchOrderAsync(searchQuery);
        }

        public async Task UpdateOrderHeaderAsync(OrderHeaderUpdateDTO orderHeaderUpdateDTO)
        {
            await ApiClient.UpdateOrderHeaderAsync(orderHeaderUpdateDTO);
        }

        public async Task UpdateOrderLineAsync(OrderPartListUpdate orderPartListUpdateDTO)
        {
            await ApiClient.UpdateOrderLineAsync(orderPartListUpdateDTO);
        }

        public async Task UpdateOrderShippingAsync(OrderShipUpdateDTO orderShipUpdateDTO)
        {
            await ApiClient.UpdateOrderShippingAsync(orderShipUpdateDTO);
        }
    }
}
