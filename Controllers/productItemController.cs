using AutoMapper;
using CoolMate.DTO;
using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using CoolMate.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoolMate.Controllers
{
    [Route("api/[controller]")]
    public class productItemController : ControllerBase
    {
        private readonly IProductItemRepository _productItemRepository;
        private readonly CloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        public productItemController(IProductItemRepository productItemRepository, IMapper mapper, CloudinaryService cloudinaryService)
        {
            _productItemRepository = productItemRepository;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Get(int id)
        {
            var productItem = await _productItemRepository.GetProductItemByIdAsync(id);
            if (productItem == null) return NotFound("not found");

            return Ok(_mapper.Map<ProductItemDTO>(productItem));
        }

        [HttpPut("updateQtyInStock")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> UpdateQtyInStock([FromBody] UpdateQtyInStockDTO updateQtyInStock)
        {
            var productItem = await _productItemRepository.GetProductItemByIdAsync(updateQtyInStock.ProductItemId);
            if (productItem == null) return NotFound("not found");
            productItem.QtyInStock = updateQtyInStock.newQty;
            await _productItemRepository.UpdateProductItemAsync(productItem);
            return Ok("successfully");
        }

        [HttpPost("add")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddProductItem([FromForm] CreateProductItemDTO createProductItem)
        {
            List<string> imageUrls = null;
            foreach (var size in createProductItem.Size)
            {
                var productItem = new ProductItem
                {
                    ProductId = createProductItem.ProductId,
                    Size = size,
                    Color = createProductItem.Color.color,
                    ColorImage = createProductItem.Color.url,
                    QtyInStock = createProductItem.Qty
                };
                if (imageUrls == null)
                    imageUrls = await _cloudinaryService.UploadImagesAsync(createProductItem.Images);
                foreach (var imageUrl in imageUrls)
                {
                    productItem.ProductItemImages.Add(new ProductItemImage { Url = imageUrl });
                }
                await _productItemRepository.CreateProductItemAsync(productItem);
            }
            return Ok("successfully");
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteProductItem([FromBody] List<int> productItemIds)
        {
            List<ProductItem> productItems = new List<ProductItem>();
            foreach (int productItemId in productItemIds)
            {
                var productItem = await _productItemRepository.GetProductItemByIdAsync(productItemId);
                if (productItem != null) productItems.Add(productItem);
            }
            foreach (ProductItem productItem in  productItems)
            {
                await _productItemRepository.DeleteProductItemAsync(productItem);
            }
            return Ok($"deleted {productItems.Count()} items");
        }
    }
}
