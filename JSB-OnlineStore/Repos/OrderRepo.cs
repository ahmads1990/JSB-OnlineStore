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
    public Task UpdateOrderAsync(int id, Order order)
    {
        throw new NotImplementedException();
    }
    public Task DeleteOrderAsync(int id)
    {
        throw new NotImplementedException();
    }
}
