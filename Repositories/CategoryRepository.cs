using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoolMate.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DBContext _dbContext;
        public CategoryRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task createCategoryAsync(ProductCategory category)
        {
            await _dbContext.ProductCategories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(ProductCategory category)
        {
            _dbContext.ProductCategories.Remove(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ProductCategory> GetByIdAsync(int id)
        {
            return await _dbContext.ProductCategories.FindAsync(id);
        }

        public async Task<List<ProductCategory>> GetCategoriesAsync()
        {
            return await _dbContext.ProductCategories.ToListAsync();
        }

        public async Task updateCategoryAsync(ProductCategory category)
        {
            _dbContext.ProductCategories.Update(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}
