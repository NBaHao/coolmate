using CoolMate.Models;

namespace CoolMate.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductAsync(int productId);
        Task<List<Product>> GetProductsByCategoryAsync(string category);
        Task CreateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int productId);
        Task<bool> UpdateProductAsync(Product product);
        Task<List<Product>> GetAllProducts();
    }
}
