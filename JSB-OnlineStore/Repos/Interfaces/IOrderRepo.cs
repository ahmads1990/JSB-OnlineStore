using JSB_OnlineStore.Models;

namespace JSB_OnlineStore.Repos.Interfaces;

public interface IOrderRepo
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order?> GetOrderByIDAsync(int id);
    Task<Order> CreateOrderAsync(Order order);
    Task UpdateOrderAsync(int id, Order order);
    Task DeleteOrderAsync(int id);
}
