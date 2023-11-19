using CoolMate.Models;

namespace CoolMate.Repositories.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> GetByUserIdAsync(string userId);
        Task<bool> AddToCartItemAsync(string userId, int productItemId, int quantity);
        Task<bool> UpdateQtyCartItemAsync(string userId, int productItemId, int qty);
        Task<bool> RemoveCartItemAsync(string userId, int productItemId);
        Task<bool> ReplaceCartItemAsync(string userId, int oldProductItemId, int newProductItemId);
        Task<ShoppingCart> GetFullInfomationAsync(string userId);
    }
}
