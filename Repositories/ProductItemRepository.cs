using CoolMate.DTO;
using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoolMate.Repositories
{
    public class ProductItemRepository : IProductItemRepository
    {
        private readonly DBContext _dbContext;
        public ProductItemRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateProductItemAsync(ProductItem productItem)
        {
            await _dbContext.ProductItems.AddAsync(productItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProductItemAsync(ProductItem productItem)
        {
            _dbContext.ProductItems.Remove(productItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ProductItem> GetProductItemByIdAsync(int id)
        {
            return await _dbContext.ProductItems.Include(pi => pi.ProductItemImages).Where(pi => pi.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateProductItemAsync(ProductItem productItem)
        {
            _dbContext.ProductItems.Update(productItem);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateQtyInStockAsync(List<OrderLine> ordersLine)
        {
            foreach (var orderLine in ordersLine)
            {
                var productItem = await _dbContext.ProductItems.FindAsync(orderLine.ProductItemId);
                productItem.QtyInStock -= orderLine.Qty;
                _dbContext.ProductItems.Update(productItem);
            }
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<getAllItemDTO>> GetProductItemsAsync()
        {
            return await _dbContext.ProductItems.Include(pi => pi.Product).Select(pi => new getAllItemDTO
            {
                Id = pi.Id,
                ProductId = pi.ProductId,
                name = pi.Product.Name,
                Size = pi.Size,
                Color = pi.Color,
                ColorImage = pi.ColorImage,
                QtyInStock = pi.QtyInStock,
                priceStr = pi.Product.PriceStr
            }).ToListAsync();
        }
    }
}
