using CoolMate.DTO;
using CoolMate.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoolMate.Controllers
{
    [Route("api/[controller]")]
    public class statisticController : ControllerBase
    {
        private readonly IShopOrderRepository _shopOrderRepository;
        public statisticController(IShopOrderRepository shopOrderRepository)
        {
            _shopOrderRepository = shopOrderRepository;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("bestsellers")]
        public async Task<ActionResult> GetBestSellers()
        {
            var orders = await _shopOrderRepository.GetShopOrdersAsync();
            var response = orders.SelectMany(o => o.OrderLines).GroupBy(ol => ol.ProductItemId).Select(g => new
            {
                ProductItemId = g.Key,
                Quantity = g.Sum(x => x.Qty)
            }).OrderByDescending(x => x.Quantity).Take(5);
            return Ok(response);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("lifetimesales")]
        public async Task<ActionResult> GetLifeTimeSales()
        {
            var orders = await _shopOrderRepository.GetShopOrdersAsync();
            var response = new LifetimeSalesDTO
            {
                TotalSales = orders.Sum(o => (long)o.OrderTotal),
                TotalOrders = orders.Count()
            };
            response.CompletedPercentage = orders.Where(o => o.OrderStatus == 1).Count() / response.TotalOrders;
            response.CancelledPercentage = orders.Where(o => o.OrderStatus == 2).Count() / response.TotalOrders;
            return Ok(response);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("salestatistic")]
        public async Task<ActionResult> GetSaleStatistic([FromQuery] string period)
        {
            var orders = await _shopOrderRepository.GetShopOrdersAsync();
            var response = new SaleStatisticDTO
            {
                data = new List<OneSaleStatisticDTO>()
            };
            switch (period)
            {
                case "daily":
                    var sixDaysAgo = DateTime.Now.AddDays(-6);
                    response.data = orders.Where(o => o.OrderDate >= sixDaysAgo).GroupBy(o => new { o.OrderDate.Value.Date, o.OrderDate.Value.Month, o.OrderDate.Value.Year }).Select(g => new OneSaleStatisticDTO
                    {
                        total = g.Sum(x => (long)x.OrderTotal),
                        count = g.Count(),
                        time = g.Key.Date.ToString() + "/" + g.Key.Month.ToString() + "/" + g.Key.Year.ToString()
                    }).ToList();

                    break;
                case "weekly":
                    var sixWeeksAgo = DateTime.Now.AddDays(-42); 
                    response.data = orders
                        .Where(o => o.OrderDate >= sixWeeksAgo)
                        .GroupBy(o => new { WeekStart = o.OrderDate.Value.Date.AddDays(-((int)o.OrderDate.Value.DayOfWeek - 1)), o.OrderDate.Value.Month, o.OrderDate.Value.Year })
                        .Select(g => new OneSaleStatisticDTO
                        {
                            total = g.Sum(x => (long)x.OrderTotal),
                            count = g.Count(),
                            time = $"{g.Key.WeekStart}/{g.Key.Month}/{g.Key.Year}"
                        })
                        .ToList();
                    break;
                case "monthly":
                    var sixMonthsAgo = DateTime.Now.AddMonths(-6);
                    response.data = orders.Where(o => o.OrderDate >= sixMonthsAgo).GroupBy(o => new { o.OrderDate.Value.Month, o.OrderDate.Value.Year }).Select(g => new OneSaleStatisticDTO
                    {
                        total = g.Sum(x => (long)x.OrderTotal),
                        count = g.Count(),
                        time = g.Key.Month.ToString() + "/" + g.Key.Year.ToString()
                    }).ToList();
                    break;
                default:
                    break;
            }
            return Ok(response);
        }
    }
}
