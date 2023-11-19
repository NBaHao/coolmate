using CoolMate.Models;

namespace CoolMate.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<ProductCategory> GetByIdAsync(int id);
        Task<List<ProductCategory>> GetCategoriesAsync();
        Task createCategoryAsync(ProductCategory category);
        Task updateCategoryAsync(ProductCategory category);
        Task DeleteCategoryAsync(ProductCategory category);
    }
}
