using CoolMate.DTO;
using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoolMate.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly DBContext _dbContext;

        public ShoppingCartRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ShoppingCart> GetFullInfomationAsync(string userId)
        {
            var shoppingCart = _dbContext.ShoppingCarts
            .Include(sc => sc.ShoppingCartItems)
            .ThenInclude(sci => sci.ProductItem)
            .ThenInclude(pi => pi.Product)
            .ThenInclude(p => p.ProductItems)
            .ThenInclude(pi => pi.ProductItemImages)
            .Where(sc => sc.UserId == userId)
            .FirstOrDefault();

            if (shoppingCart == null)
            {
                return null;
            }

            return shoppingCart;
        }

        public async Task<ShoppingCart> GetByUserIdAsync(string userId)
        {
            return await _dbContext.ShoppingCarts
                .Include(sc => sc.ShoppingCartItems)
                .FirstOrDefaultAsync(sc => sc.UserId == userId);
        }

        public async Task AddToCartItemAsync(string userId, int productItemId)
        {
            var cart = await GetByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new ShoppingCart { UserId = userId };
                _dbContext.ShoppingCarts.Add(cart);
                await _dbContext.SaveChangesAsync();
            }

            var existingItem = cart.ShoppingCartItems.FirstOrDefault(item => item.ProductItemId == productItemId);
            if (existingItem != null)
            {
                existingItem.Qty = existingItem.Qty + 1;
            }
            else
            {
                var newItem = new ShoppingCartItem { CartId = cart.Id, ProductItemId = productItemId, Qty = 1 };
                _dbContext.ShoppingCartItems.Add(newItem);
            }

            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateQtyCartItemAsync(string userId, int productItemId, int qty)
        {
            var cart = await GetByUserIdAsync(userId);
            if (cart == null)
            {
                return;
            }

            var existingItem = cart.ShoppingCartItems.FirstOrDefault(item => item.ProductItemId == productItemId);
            if (existingItem != null)
            {
                existingItem.Qty = qty;
            }
            else   //Cho phép tự động thêm dù không có sản phẩm này (productItemId) trong giỏ
            {
                var newItem = new ShoppingCartItem { CartId = cart.Id, ProductItemId = productItemId, Qty = qty };
                _dbContext.ShoppingCartItems.Add(newItem);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> RemoveCartItemAsync(string userId, int productItemId)
        {
            var cart = await GetByUserIdAsync(userId);
            if (cart == null)
            {
                return false;
            }

            var itemToRemove = cart.ShoppingCartItems.FirstOrDefault(item => item.ProductItemId == productItemId);
            if (itemToRemove == null)
            {
                return false;
            }

            _dbContext.ShoppingCartItems.Remove(itemToRemove);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReplaceCartItemAsync(string userId, int oldProductItemId, int newProductItemId)
        {
            var cart = await GetByUserIdAsync(userId);
            if (cart == null)
            {
                return false;
            }

            var oldItem = cart.ShoppingCartItems.FirstOrDefault(item => item.ProductItemId == oldProductItemId);
            if (oldItem == null)
            {
                return false;
            }

            oldItem.ProductItemId = newProductItemId;

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
