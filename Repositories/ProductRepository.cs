using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoolMate.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DBContext _dbContext;
        public ProductRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateProductAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            //
            return true;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _dbContext.Products
                        .Include(p=>p.ProductItems)
                        .ThenInclude(pi=>pi.ProductItemImages)
                        .ToListAsync();
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            return await _dbContext.Products
                        .Include(p => p.ProductItems)
                        .ThenInclude(pi => pi.ProductItemImages)
                        .Where(p => p.Id == productId)
                        .FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(string category)
        {
            return await _dbContext.Products
                        .Include(p => p.ProductItems)
                        .ThenInclude(pi => pi.ProductItemImages)
                        .Where(p => p.Category.CategoryName == category)
                        .ToListAsync();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        { 
            var res = _dbContext.Products.Update(product);
            if (res != null) return true; 
            return false;
        }
    }
}
