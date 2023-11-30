using CoolMate.DTO;
using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoolMate.Controllers
{
    [Route("api/[controller]")]
    public class orderController : ControllerBase
    {
        private readonly IShopOrderRepository _shopOrderRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        public orderController(IShopOrderRepository shopOrderRepository, IShoppingCartRepository shoppingCartRepository)
        {
            _shopOrderRepository = shopOrderRepository;
            _shoppingCartRepository = shoppingCartRepository;
        }

        [HttpGet("getAll")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> GetAllOrders()
        {
            var orders = await _shopOrderRepository.GetShopOrdersAsync();
            var response = orders.Select(o => new ShopOrderDTO
            {
                Id = o.Id,
                UserId = o.UserId,
                OrderDate = o.OrderDate,
                OrderStatus = o.OrderStatus,
                PaymentMethod = o.PaymentMethod,
                ShippingAddress = o.ShippingAddress,
                ShippingMethod = o.ShippingMethod,
                OrderTotal = o.OrderTotal,
                OrderLines = o.OrderLines.Select(ol => new OrderLineDTO
                {
                    Id = ol.Id,
                    ProductName = ol.ProductItem.Product.Name,
                    Color = ol.ProductItem.Color,
                    Size = ol.ProductItem.Size,
                    ProductItemId = ol.ProductItemId,
                    Quantity = ol.Qty,
                    Price = ol.Price,
                    Img = ol.ProductItem.ProductItemImages.FirstOrDefault()?.Url
                })
            });
            return Ok(response);
        }

        [HttpGet("get")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult> GetOrdersOfUser()
        {
            var orders = await _shopOrderRepository.GetOrdersOfUserAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response = orders.Select(o => new ShopOrderDTO
            {
                Id = o.Id,
                UserId = o.UserId,
                OrderDate = o.OrderDate,
                OrderStatus = o.OrderStatus,
                PaymentMethod = o.PaymentMethod,
                ShippingAddress = o.ShippingAddress,
                ShippingMethod = o.ShippingMethod,
                OrderTotal = o.OrderTotal,
                OrderLines = o.OrderLines.Select(ol => new OrderLineDTO
                {
                    Id = ol.Id,
                    ProductName = ol.ProductItem.Product.Name,
                    Color = ol.ProductItem.Color,
                    Size = ol.ProductItem.Size,
                    ProductItemId = ol.ProductItemId,
                    Quantity = ol.Qty,
                    Price = ol.Price,
                    Img = ol.ProductItem.ProductItemImages.FirstOrDefault()?.Url
                })
            });
            return Ok(response);
        }

        [HttpPut("updateStatus")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusDTO updateOrderStatusDTO)
        {
            var res = await _shopOrderRepository.updateOrderStatusAsync(updateOrderStatusDTO.orderId, updateOrderStatusDTO.status);
            if (res) return Ok("successfully");
            return BadRequest("failed");
        }

        [HttpPost("create")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult> CreateOrder([FromBody] CreateOrderDTO createOrderDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var shoppingCart = await _shoppingCartRepository.GetFullInfomationAsync(userId);
            if (shoppingCart == null || shoppingCart.ShoppingCartItems.Count() == 0)
                return NotFound("there's no item in cart");

            var order = new ShopOrder
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                OrderStatus = 0,
                PaymentMethod = createOrderDTO.paymentMethod,
                ShippingAddress = createOrderDTO.shippingAddress,
                ShippingMethod = createOrderDTO.shippingMethod,
                OrderLines = shoppingCart.ShoppingCartItems.Select(sci => new OrderLine
                {
                    ProductItemId = sci.ProductItem.Id,
                    Qty = sci.Qty,
                    Price = sci.ProductItem.Product.PriceInt ?? 0
                }).ToList(),
                OrderTotal = shoppingCart.ShoppingCartItems.Sum(sci => sci.ProductItem.Product.PriceInt * sci.Qty)
            };
            await _shopOrderRepository.createShopOrderAsync(order);
            await _shoppingCartRepository.RemoveAllItemsInCartAsync(userId);
            return Ok("successfully");
        }

    }
}
