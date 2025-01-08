using Test_Consimple.Entity.Product;
using Test_Consimple.Entity.Product.Repository;
using Test_Consimple.Models.ProductModels;

namespace Test_Consimple.Service;

public interface IProductService
{
    Task<IEnumerable<ProductResponse>> GetAllAsync();
    Task<ProductResponse> GetByIdAsync(Guid id);
    Task CreateAsync(CreateProductRequest request);
    Task UpdateAsync(Guid id, UpdateProductRequest request);
    Task DeleteAsync(Guid id);
}


public class ProductService : IProductService
{
    private readonly IProductRepository<Product> _productRepository;

    public ProductService(IProductRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductResponse>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(p => new ProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Category = p.Category,
            SKU = p.SKU,
            Price = p.Price
        });
    }

    public async Task<ProductResponse> GetByIdAsync(Guid id)
    {
        var product = await _productRepository.FindByIdAsync(id);
        if (product == null)
            throw new KeyNotFoundException("Product not found.");

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Category = product.Category,
            SKU = product.SKU,
            Price = product.Price
        };
    }

    public async Task CreateAsync(CreateProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Category = request.Category,
            SKU = request.SKU,
            Price = request.Price
        };

        await _productRepository.InsertOneAsync(product);
    }

    public async Task UpdateAsync(Guid id, UpdateProductRequest request)
    {
        var product = await _productRepository.FindByIdAsync(id);
        if (product == null)
            throw new KeyNotFoundException("Product not found.");

        product.Name = request.Name;
        product.Category = request.Category;
        product.SKU = request.SKU;
        product.Price = request.Price;

        await _productRepository.UpdateOneAsync(product);
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _productRepository.FindByIdAsync(id);
        if (product == null)
            throw new KeyNotFoundException("Product not found.");

        await _productRepository.DeleteOneAsync(id);
    }
}