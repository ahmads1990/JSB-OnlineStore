using JSB_OnlineStore.Data;
using JSB_OnlineStore.Models;
using JSB_OnlineStore.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JSB_OnlineStore.Repos;

public class OrderRepo : IOrderRepo
{
    private readonly AppDbContext _appDbContext;

    public OrderRepo(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        return await _appDbContext.Orders.ToListAsync();
    }
    public async Task<Order?> GetOrderByIDAsync(int id)
    {
        return await _appDbContext.Orders.FirstOrDefaultAsync(o => o.OrderID == id);
    }
    public async Task<Order> CreateOrderAsync(Order order)
    {
        var result = await _appDbContext.Orders.AddAsync(order);
        return result.Entity;
    }
    public async Task UpdateOrderAsync(int id, Order order)
    {
        var existingOrder = await GetOrderByIDAsync(id);

        if (existingOrder is null)
            throw new Exception($"Order not found for ID: {id}");

        existingOrder.CustomerID = order.CustomerID;
        foreach (var orderItem in order.Items)
        {
            existingOrder.Items
                .First(i => i.OrderItemID == orderItem.OrderItemID)
                .Quantity = orderItem.Quantity;
        }

        _appDbContext.Orders.Update(existingOrder);
        await _appDbContext.SaveChangesAsync();
    }
    public async Task DeleteOrderAsync(int id)
    {
        var existingOrder = await GetOrderByIDAsync(id);

        if (existingOrder is null)
            throw new Exception($"Order not found for ID: {id}");

        _appDbContext.Orders.Remove(existingOrder);
    }
}
