using JSB_OnlineStore.Data;
using JSB_OnlineStore.Models;
using JSB_OnlineStore.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JSB_OnlineStore.Repos;

public class ProductRepo : IProductRepo
{
    private readonly AppDbContext _appDbContext;
    public ProductRepo(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _appDbContext.Products
                        .AsNoTracking()
                        .ToListAsync();
    }
    public async Task<Product?> GetProductByIDAsync(int id)
    {
        return await _appDbContext.Products
                     .AsNoTracking()
                     .FirstOrDefaultAsync(p => p.ProductID == id);
    }
    public async Task<Product> CreateProductAsync(Product product)
    {
        var result = await _appDbContext.Products.AddAsync(product);
        return result.Entity;
    }
    public async Task UpdateProductAsync(int id, Product product)
    {
        var existingProduct = await GetProductByIDAsync(id);

        if (existingProduct is null)
            throw new Exception($"Product not found for ID: {id}");

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;

        _appDbContext.Products.Update(existingProduct);
        await _appDbContext.SaveChangesAsync();
    }
    public async Task CheckAndUpdateProductsStock(OrderItem orderItem, int newQuantity)
    {
        var productOrdered = await _appDbContext.Products
             .FirstOrDefaultAsync(product => product.ProductID == orderItem.ProductID);

        if (productOrdered is null)
            throw new Exception($"Order product can't be found invalid ID: [{orderItem.ProductID}]");

        // Check if there is enough stock for the ordered quantity
        var productStockChange = orderItem.Quantity - newQuantity;

        // Add add back to product stock
        if (productStockChange > 0)
            productOrdered.Stock += productStockChange;
        else if (productStockChange < 0)
        {
            var requiredStock = Math.Abs(productStockChange);
            if (productOrdered.Stock < requiredStock)
                throw new Exception($"Not enough stock for Product ID: {productOrdered.ProductID}. Available: {productOrdered.Stock}, Ordered: {newQuantity}");

            // Take from stock
            productOrdered.Stock -= requiredStock;
        }
        await _appDbContext.SaveChangesAsync();
    }
    public async Task CheckAndUpdateProductsStock(List<OrderItem> orderItems)
    {
        var orderProductIDs = orderItems.Select(item => item.ProductID);

        var productsToBeOrdered = await _appDbContext.Products
             .Where(product => orderProductIDs.Contains(product.ProductID))
             .ToListAsync();

        if (productsToBeOrdered.Count < orderItems.Count)
        {
            var foundProductIDs = productsToBeOrdered.Select(p => p.ProductID);
            var invalidIDs = orderProductIDs.Except(orderProductIDs);
            throw new Exception($"Some Order products can't be found invalid ID: [{string.Join(",", invalidIDs)}]");
        }

        // Update each product stock
        foreach (var orderItem in orderItems)
        {
            var product = productsToBeOrdered.First(p => p.ProductID == orderItem.ProductID);

            // Check if there is enough stock for the ordered quantity
            if (product.Stock < orderItem.Quantity)
            {
                throw new Exception($"Not enough stock for Product ID: {product.ProductID}. Available: {product.Stock}, Ordered: {orderItem.Quantity}");
            }
            product.Stock -= orderItem.Quantity;
        }

        await _appDbContext.SaveChangesAsync();
    }
    public async Task DeleteProductAsync(int id)
    {
        var existingProduct = await GetProductByIDAsync(id);

        if (existingProduct is null)
            throw new Exception($"Product not found for ID: {id}");

        _appDbContext.Products.Remove(existingProduct);
        await _appDbContext.SaveChangesAsync();
    }
}
