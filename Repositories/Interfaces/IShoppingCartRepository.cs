using CoolMate.Models;

namespace CoolMate.Repositories.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> GetByUserIdAsync(string userId);
        Task AddToCartItemAsync(string userId, int productItemId);
        Task UpdateQtyCartItemAsync(string userId, int productItemId, int qty);
        Task<bool> RemoveCartItemAsync(string userId, int productItemId);
        Task<bool> ReplaceCartItemAsync(string userId, int oldProductItemId, int newProductItemId);
        Task<ShoppingCart> GetFullInfomationAsync(string userId);
    }
}
