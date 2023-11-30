using CoolMate.Models;

namespace CoolMate.Repositories.Interfaces
{
    public interface IShopOrderRepository
    {
        Task<IEnumerable<ShopOrder>> GetShopOrdersAsync();
        Task<ShopOrder> GetByIdAsync(int id);
        Task<IEnumerable<ShopOrder>> GetOrdersOfUserAsync(string userId);
        Task createShopOrderAsync(ShopOrder shopOrder);
        Task<bool> updateOrderStatusAsync(int orderId, int status);
        Task<bool> IsProductItemOrderedAsync(string? userId, int? ProductId);
    }
}
