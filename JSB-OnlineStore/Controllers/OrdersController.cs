using JSB_OnlineStore.Dtos.Order;
using JSB_OnlineStore.Dtos.OrderItem;
using JSB_OnlineStore.Models;
using JSB_OnlineStore.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JSB_OnlineStore.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IProductRepo _productRepo;
    private readonly IOrderRepo _orderRepo;

    public OrdersController(IProductRepo productRepo, IOrderRepo orderRepo)
    {
        _productRepo = productRepo;
        _orderRepo = orderRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderRepo.GetOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("{orderID}")]
    public async Task<IActionResult> GetOrderById(int orderID)
    {
        var order = await _orderRepo.GetOrderByIDAsync(orderID);
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder([FromBody] AddOrderDto addOrderDto)
    {
        var order = new Order
        {
            CustomerID = addOrderDto.CustomerID,
            OrderDate = DateTime.Now,
            Items = addOrderDto.Items.Select(item =>
                new OrderItem
                {
                    ProductID = item.ProductID,
                    Price = item.Price,
                    Quantity = item.Quantity
                }).ToList()
        };

        try
        {
            await _productRepo.CheckAndUpdateProductsStock((List<OrderItem>)order.Items);
            await _orderRepo.CreateOrderAsync(order);
        }
        catch (Exception)
        {
            return BadRequest();
        }
        return Ok();
    }
    [HttpPut]
    public async Task<IActionResult> UpdateOrder(int orderID, UpdateOrderDto updateOrderDto)
    {
        var order = await _orderRepo.GetOrderByIDAsync(orderID);

        if (order == null)
            return NotFound();

        order.CustomerID = updateOrderDto.CustomerID;
        try
        {
            foreach (var item in updateOrderDto.Items)
            {
                var orderItem = order.Items.FirstOrDefault(i => i.ProductID == item.ProductID);
                if (orderItem != null)
                {
                    await _productRepo.CheckAndUpdateProductsStock(orderItem, item.Quantity);
                    orderItem.Quantity = item.Quantity;
                }
                else
                {
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProduct(int productID)
    {
        try
        {
            await _productRepo.DeleteProductAsync(productID);
        }
        catch (Exception)
        {
            return NotFound();
        }
        return Ok();
    }
}
