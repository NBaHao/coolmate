using CoolMate.DTO;
using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoolMate.Controllers
{
    [Route("api/[controller]")]
    public class cartController : ControllerBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        public cartController(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        [Authorize]
        [HttpGet()]
        public async Task<ActionResult> GetFullInfomation()
        {
            var shoppingCart = await _shoppingCartRepository.GetFullInfomationAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (shoppingCart == null)
                return NotFound();

            var response = shoppingCart.ShoppingCartItems.Select(sci => new ShoppingCartDTO
            {
                ProductItemId = sci.ProductItemId,
                ProductId = sci.ProductItem.ProductId,
                Name = sci.ProductItem.Product.Name,
                Price = sci.ProductItem.Product.PriceInt ?? 0,
                Color = sci.ProductItem.Color,
                Size = sci.ProductItem.Size,
                Img = sci.ProductItem.Product.ProductItems.Where(pi=>pi.Id == sci.ProductItemId).FirstOrDefault()?.ProductItemImages.FirstOrDefault()?.Url,
                Qty = sci.Qty ?? 0,
                AllItemsOfProduct = sci.ProductItem.Product.ProductItems.Select(pi => new ProductItemDetailsDTO
                {
                    ProductItemId = pi.Id,
                    Size = pi.Size,
                    Color = pi.Color
                }).ToList()
            }).ToList();

            return Ok(response);
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult> AddToCart([FromBody] int productItemId)
        {
            await _shoppingCartRepository.AddToCartItemAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, productItemId);
            return Ok("successfully...or not");
        }

        [Authorize]
        [HttpPut("updateQty")]
        public async Task<ActionResult> UpdateQtyCartItem([FromBody] UpdateQtyCartItemDTO updateQtyCartItemDTO)
        {
            if (updateQtyCartItemDTO.quantity <= 0) return BadRequest("qty must > 0");
            await _shoppingCartRepository.UpdateQtyCartItemAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, updateQtyCartItemDTO.productItemId, updateQtyCartItemDTO.quantity);
            return Ok("successfully...or not");
        }

        [Authorize]
        [HttpDelete("removeCartItem")]
        public async Task<ActionResult> RemoveCartItem([FromBody] int productItemId)
        {
            var res = await _shoppingCartRepository.RemoveCartItemAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, productItemId);
            if (res) return Ok("successfully");
            return BadRequest("something went wrong");
        }

        [Authorize]
        [HttpPut("replaceCartItem")]
        public async Task<ActionResult> ReplaceCartItem([FromBody] ReplaceCartItemDTO replaceCartItemDTO)
        {
            var res = await _shoppingCartRepository.ReplaceCartItemAsync(
                    User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    replaceCartItemDTO.oldProductItemId,
                    replaceCartItemDTO.newProductItemId);
            if (res) return Ok("successfully");
            return BadRequest("something went wrong");
        }
    }
}
