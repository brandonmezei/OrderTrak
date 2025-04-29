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

        public async Task<OrderHeaderDTO> GetOrderHeaderAsync(Guid orderID)
        {
            return await ApiClient.GetOrderHeaderAsync(orderID);
        }

        public async Task<PagedTableOfOrderSearchReturnDTO> SearchOrderAsync(SearchQueryDTO searchQuery)
        {
            return await ApiClient.SearchOrderAsync(searchQuery);
        }

        public async Task UpdateOrderHeaderAsync(OrderHeaderUpdateDTO orderHeaderUpdateDTO)
        {
            await ApiClient.UpdateOrderHeaderAsync(orderHeaderUpdateDTO);
        }
    }
}
