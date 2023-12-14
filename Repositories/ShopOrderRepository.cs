using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoolMate.Repositories
{
    public class ShopOrderRepository : IShopOrderRepository
    {
        private readonly DBContext _context;
        public ShopOrderRepository(DBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ShopOrder>> GetShopOrdersAsync()
        {
            return await _context.ShopOrders
                .Include(x => x.User)
                .Include(x => x.OrderLines)
                .ThenInclude(x => x.ProductItem)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductItems)
                .ThenInclude(x => x.ProductItemImages)
                .ToListAsync();
        }
        public async Task<IEnumerable<ShopOrder>> GetOrdersOfUserAsync(string userId)
        {
            return await _context.ShopOrders
                .Include(x => x.User)
                .Include(x => x.OrderLines)
                .ThenInclude(x => x.ProductItem)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductItems)
                .ThenInclude(x => x.ProductItemImages)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
        public async Task<ShopOrder> GetByIdAsync(int id)
        {
            return await _context.ShopOrders
                .Include(x => x.User)
                .Include(x => x.OrderLines)
                .ThenInclude(x => x.ProductItem)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task createShopOrderAsync(ShopOrder shopOrder)
        {
            await _context.ShopOrders.AddAsync(shopOrder);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> updateOrderStatusAsync(int orderId, int status)
        {
            var order = await _context.ShopOrders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order == null) return false;
            order.OrderStatus = status;
            await _context.SaveChangesAsync();
            return true;
        }   
        public async Task<bool> IsProductItemOrderedAsync(string? userId, int? ProductItemId)
        {
            var order = await _context.ShopOrders
                .Include(x => x.OrderLines)
                .ThenInclude(x => x.ProductItem)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.OrderStatus == 1);
            if (order == null) return false;
            return order.OrderLines.Any(x => x.ProductItem.Id == ProductItemId);
        }
        public async Task RemoveOrderAsync(ShopOrder order)
        {
            _context.ShopOrders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
