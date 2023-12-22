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
            _dbContext.Products.Remove(await _dbContext.Products.FindAsync(productId));
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _dbContext.Products.ToListAsync();
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
            var parentCateId = (await _dbContext.ProductCategories
                .Where(cate => cate.Slug == category)
                .FirstOrDefaultAsync()).Id;
            var listChildCate = await _dbContext.ProductCategories
                .Where(cate => cate.ParentCategoryId == parentCateId || cate.Id == parentCateId)
                .Select(cate => cate.Slug)
                .ToListAsync();  
            return await _dbContext.Products
                        .Include(p => p.ProductItems)
                        .ThenInclude(pi => pi.ProductItemImages)
                        .Where(p => listChildCate.Contains(p.Category.Slug))
                        .ToListAsync();
        }

        public async Task<List<Product>> SearchProductAsync(string searchTerm)
        {
            return await _dbContext.Products
                        .Include(p => p.ProductItems)
                        .ThenInclude(pi => pi.ProductItemImages)
                        .Where(p => p.Name.Contains(searchTerm))
                        .ToListAsync();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var res = _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
            if (res != null) return true;
            return false;
        }

        public async Task<List<Product>> GetProductsByCategoryWithFilterAsync(string category, string filter)
        {
            var parentCateId = (await _dbContext.ProductCategories
                .Where(cate => cate.Slug == category)
                .FirstOrDefaultAsync()).Id;
            var listChildCate = await _dbContext.ProductCategories
                .Where(cate => cate.ParentCategoryId == parentCateId || cate.Id == parentCateId)
                .Select(cate => cate.Slug)
                .ToListAsync();

            switch (filter)
            {
                case "gia-thap-den-cao":
                    return await _dbContext.Products
                        .Include(p => p.ProductItems)
                        .ThenInclude(pi => pi.ProductItemImages)
                        .Where(p => listChildCate.Contains(p.Category.Slug))
                        .OrderBy(p => p.PriceInt)
                        .ToListAsync();
                case "gia-cao-den-thap":
                    return await _dbContext.Products
                        .Include(p => p.ProductItems)
                        .ThenInclude(pi => pi.ProductItemImages)
                        .Where(p => listChildCate.Contains(p.Category.Slug))
                        .OrderByDescending(p => p.PriceInt)
                        .ToListAsync();
                case "a-z":
                    return await _dbContext.Products
                        .Include(p => p.ProductItems)
                        .ThenInclude(pi => pi.ProductItemImages)
                        .Where(p => listChildCate.Contains(p.Category.Slug))
                        .OrderBy(p => p.Name)
                        .ToListAsync();
                case "z-a":
                    return await _dbContext.Products
                        .Include(p => p.ProductItems)
                        .ThenInclude(pi => pi.ProductItemImages)
                        .Where(p => listChildCate.Contains(p.Category.Slug))
                        .OrderByDescending(p => p.Name)
                        .ToListAsync();
                case "ban-chay":
                    return await _dbContext.OrderLines
                        .Include(ol => ol.ProductItem)
                        .ThenInclude(pi => pi.Product)
                        .Where(ol => listChildCate.Contains(p.Category.Slug))
                        .GroupBy(ol => ol.ProductItem.Product)
                        .OrderByDescending(g => g.Count())
                        .Select(g => g.Key)
                        .ToListAsync();
                default:
                    return await _dbContext.Products
                        .Include(p => p.ProductItems)
                        .ThenInclude(pi => pi.ProductItemImages)
                        .Where(p => listChildCate.Contains(p.Category.Slug))
                        .ToListAsync();
            }
        }
    }
}
