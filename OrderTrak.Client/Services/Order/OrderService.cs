using OrderTrak.Client.Services.API;

namespace OrderTrak.Client.Services.Order
{
    public class OrderService(IClient client) : IOrderService
    {
        private readonly IClient ApiClient = client;

        public async Task CancelOrderAsync(OrderCancelDTO orderCancelDTO)
        {
            await ApiClient.CancelOrderAsync(orderCancelDTO);
        }

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

        public async Task<OrderActivationDTO> GetOrderActivationAsync(Guid orderID)
        {
            return await ApiClient.GetOrderActivationAsync(orderID);
        }

        public async Task<OrderHeaderDTO> GetOrderHeaderAsync(Guid orderID)
        {
            return await ApiClient.GetOrderHeaderAsync(orderID);
        }

        public async Task<List<OrderPartListDTO>> GetOrderLineAsync(OrderPartListSearchDTO orderPartListSearchDTO)
        {
            return [.. await ApiClient.GetOrderLineAsync(orderPartListSearchDTO)];
        }

        public async Task<OrderShipDTO> GetOrderShippingAsync(Guid orderID)
        {
            return await ApiClient.GetOrderShippingAsync(orderID);
        }

        public async Task<bool> IsDonePickAsync(OrderPickDoneDTO orderPickDoneDTO)
        {
            return await ApiClient.IsDonePickingAsync(orderPickDoneDTO);
        }

        public async Task PickToOrderAsync(OrderPickDTO orderPickDTO)
        {
            await ApiClient.PickToOrderAsync(orderPickDTO);
        }

        public async Task RemovePickFromOrderAsync(OrderPickRemoveDTO orderPickRemoveDTO)
        {
            await ApiClient.RemovePickFromOrderAsync(orderPickRemoveDTO);
        }

        public async Task<PagedTableOfOrderSearchReturnDTO> SearchOrderAsync(OrderSearchDTO searchQuery)
        {
            return await ApiClient.SearchOrderAsync(searchQuery);
        }

        public async Task UpdateOrderActivationAsync(OrderActivationUpdateDTO orderActivationUpdateDTO)
        {
            await ApiClient.UpdateOrderActivationAsync(orderActivationUpdateDTO);
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
