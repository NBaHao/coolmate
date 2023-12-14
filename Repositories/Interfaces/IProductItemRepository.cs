using CoolMate.DTO;
using CoolMate.Models;

namespace CoolMate.Repositories.Interfaces
{
    public interface IProductItemRepository
    {
        Task<ProductItem> GetProductItemByIdAsync(int id);
        Task UpdateProductItemAsync(ProductItem productItem);
        Task DeleteProductItemAsync(ProductItem productItem);
        Task CreateProductItemAsync(ProductItem productItem);
        Task UpdateQtyInStockAsync(List<OrderLine> ordersLine);
    }
}
