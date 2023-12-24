using CoolMate.DTO;
using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using CoolMate.Wrappers;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
        public async Task<IEnumerable<object>> GetBestSellerAsync()
        {
            return await _context.OrderLines.GroupBy(ol => ol.ProductItemId).Select(g => new
            {
                ProductItemId = g.Key,
                Quantity = g.Sum(x => x.Qty),
                Name = g.FirstOrDefault().ProductItem.Product.Name,
                Image = g.FirstOrDefault().ProductItem.ProductItemImages.FirstOrDefault().Url,
                Price = g.FirstOrDefault().ProductItem.Product.PriceStr
            }).OrderByDescending(x => x.Quantity).Take(5).ToListAsync();
        }
        public async Task<object> GetLifeTimeSalesAsync()
        {
            int TotalSales = 0;
            int CountCancelled = 0;
            int CountCompleted = 0;
            int TotalOrders = 0;

            await _context.ShopOrders.ForEachAsync(order =>
            {
                TotalOrders++;
                TotalSales += (int)order.OrderTotal;
                if (order.OrderStatus == 1)
                {
                    CountCompleted++;
                }
                else if (order.OrderStatus == 2)
                {
                    CountCancelled++;
                }
            });

            var response = new List<object>
            {
                new
                {
                    TotalSales = TotalSales,
                    TotalOrders = TotalOrders,
                    CancelledPercentage = Math.Round(((double)CountCancelled / (double)TotalOrders) * 100, 2),
                    CompletedPercentage = Math.Round(((double)CountCompleted / (double)TotalOrders) * 100, 2)
                }
            };
            return response;
        }
        public async Task<object> GetSalesByMonthAsync()
        {
            var response = new SaleStatisticDTO
            {
                data = new List<OneSaleStatisticDTO>()
            };
            var sevenMonthsAgo = DateTime.Now.AddMonths(-6);
            var baseDay = new DateTime(sevenMonthsAgo.Year, sevenMonthsAgo.Month, 1);
            response.data = _context.ShopOrders
                .Where(o => o.OrderDate >= baseDay && o.OrderDate <= DateTime.Now)
                .AsEnumerable()
                .GroupBy(o => new { o.OrderDate.Value.Month, o.OrderDate.Value.Year })
                .Select(g => new OneSaleStatisticDTO
                {
                    total = g.Sum(x => (long)x.OrderTotal),
                    count = g.Count(),
                    time = new DateTime(g.Key.Year, g.Key.Month, 1)
                }).OrderBy(o => o.time)
            .ToList();
            return response;
        }
        public async Task<object> GetSalesByWeekAsync()
        {
            var response = new SaleStatisticDTO
            {
                data = new List<OneSaleStatisticDTO>()
            };
            var sevenWeeksAgo = DateTime.Now.AddDays(-42);
            var baseDay = new DateTime(sevenWeeksAgo.Year, sevenWeeksAgo.Month, sevenWeeksAgo.Day);
            response.data = _context.ShopOrders
                .Where(o => o.OrderDate >= baseDay && o.OrderDate <= DateTime.Now)
                .AsEnumerable()
                .GroupBy(o => new { WeekofYear = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(o.OrderDate.Value, CalendarWeekRule.FirstDay, DayOfWeek.Monday), o.OrderDate.Value.Year })
                .Select(g => new OneSaleStatisticDTO
                {
                    total = g.Sum(x => (long)x.OrderTotal),
                    count = g.Count(),
                    time = new DateTime(g.Key.Year, 1, 1).AddDays((g.Key.WeekofYear - 1) * 7)
                }).OrderBy(o => o.time)
                .ToList();
            return response;
        }
        public async Task<object> GetSalesByDayAsync()
        {
            var response = new SaleStatisticDTO
            {
                data = new List<OneSaleStatisticDTO>()
            };
            var sevenDaysAgo = DateTime.Now.AddDays(-6);
            var baseDay = new DateTime(sevenDaysAgo.Year, sevenDaysAgo.Month, sevenDaysAgo.Day);
            response.data = _context.ShopOrders
                .Where(o => o.OrderDate >= baseDay && o.OrderDate <= DateTime.Now)
                .AsEnumerable()
                .GroupBy(o => new { o.OrderDate.Value.Date, o.OrderDate.Value.Month, o.OrderDate.Value.Year })
                .Select(g => new OneSaleStatisticDTO
                {
                    total = g.Sum(x => (long)x.OrderTotal),
                    count = g.Count(),
                    time = new DateTime(g.Key.Year, g.Key.Month, g.Key.Date.Day)
                }).OrderBy(o => o.time)
                .ToList();
            return response;
        }
    }
}
