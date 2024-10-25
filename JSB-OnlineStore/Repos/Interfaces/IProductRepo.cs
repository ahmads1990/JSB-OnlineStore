using JSB_OnlineStore.Models;

namespace JSB_OnlineStore.Repos.Interfaces;

public interface IProductRepo
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product?> GetProductByIDAsync(int id);
    Task<Product> CreateProductAsync(Product product);
    Task UpdateProductAsync(int id, Product product);
    Task CheckAndUpdateProductsStock(OrderItem orderItem, int newQuantity);
    Task CheckAndUpdateProductsStock(List<OrderItem> orderItems);
    Task DeleteProductAsync(int id);
}
