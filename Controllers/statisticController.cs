using CoolMate.DTO;
using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Globalization;

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
            var response = await _shopOrderRepository.GetBestSellerAsync();
            return Ok(response);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("lifetimesales")]
        public async Task<ActionResult> GetLifeTimeSales()
        {
            var response = await _shopOrderRepository.GetLifeTimeSalesAsync();
            return Ok(response);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("salestatistic")]
        public async Task<ActionResult> GetSaleStatistic([FromQuery] string period)
        {
            switch (period)
            {
                case "daily":
                    var response = await _shopOrderRepository.GetSalesByDayAsync();
                    return Ok(response);
                case "weekly":
                    var response1 = await _shopOrderRepository.GetSalesByWeekAsync();
                    return Ok(response1);
                case "monthly":
                    var response2 = await _shopOrderRepository.GetSalesByMonthAsync();
                    return Ok(response2);
                default:
                    return BadRequest();
            }
        }
    }
}
