using JSB_OnlineStore.Dtos;
using JSB_OnlineStore.Models;
using JSB_OnlineStore.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JSB_OnlineStore.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepo _productRepo;

    public ProductsController(IProductRepo productRepo)
    {
        _productRepo = productRepo;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productRepo.GetProductsAsync();
        return Ok(products);
    }
    [HttpGet("{productID}")]
    public async Task<IActionResult> GetProductById(int productID)
    {
        var products = await _productRepo.GetProductByIDAsync(productID);
        return Ok(products);
    }
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody]AddProductDto addProductDto)
    {
        var product = new Product
        {
            Name = addProductDto.Name,
            Description = addProductDto.Description,
            Price = addProductDto.Price,
            Stock = addProductDto.Stock
        };

        var createdProduct = await _productRepo.CreateProductAsync(product);

        return Ok(createdProduct);
    }
    [HttpPut]
    public async Task<IActionResult> UpdateProduct(int productID, [FromBody] UpdateProductDto updateProductDto)
    {
        var product = new Product
        {
            Name = updateProductDto.Name,
            Description = updateProductDto.Description,
            Price = updateProductDto.Price,
            Stock = updateProductDto.Stock
        };

        try
        {
            await _productRepo.UpdateProductAsync(productID, product);
        }
        catch (Exception)
        {
            return NotFound();
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
