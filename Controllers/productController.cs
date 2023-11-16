using CloudinaryDotNet;
using CoolMate.DTO;
using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using CoolMate.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Filter;
using WebApplication1.Helpers;
using WebApplication1.Services;

namespace CoolMate.Controllers
{
    [Route("api/[controller]")]
    public class productController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly UriService _uriService;
        private readonly CloudinaryService _cloudinaryService;
        private readonly DBContext _dbContext;

        public productController(IProductRepository productRepository, UriService uriService, CloudinaryService cloudinaryService, DBContext dBContext)
        {
            _productRepository = productRepository;
            _uriService = uriService;
            _cloudinaryService = cloudinaryService;
            _dbContext = dBContext;
        }

        [HttpGet("{category}")]
        public async Task<ActionResult> GetByCategory(string category, [FromQuery] PaginationFilter filter)
        {
            var enpointUri = $"{Request.Scheme}://{Request.Host}/api/product/{category}";
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var productsWithItemsAndImages = await _productRepository.GetProductsByCategoryAsync(category);

            foreach (var product in productsWithItemsAndImages)
            {
                // Select distinct items based on the "Color" property
                var uniqueItems = product.ProductItems.GroupBy(i => i.Color).Select(group => group.First()).ToList();

                product.ProductItems = uniqueItems;
            }

            var totalRecords = productsWithItemsAndImages.Count();

            var data = productsWithItemsAndImages
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();

            var reponse = PaginationHelper.CreatePagedReponse(data, validFilter, totalRecords, _uriService, enpointUri);
            return Ok(reponse);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("add")]
        public async Task<ActionResult> CreateProduct ([FromForm] CreateProductDTO createProductDTO)
        {
            var product = new Product
            {
                CategoryId = createProductDTO.CategoryId,
                Name = createProductDTO.Name,
                Description = createProductDTO.Description,
                PriceInt = createProductDTO.PriceInt,
                PriceStr = createProductDTO.PriceStr
            };

            foreach (var productItemDto in createProductDTO.ProductItems)
            {
                foreach (var size in productItemDto.Size)
                {
                    var productItem = new ProductItem
                    {
                        Size = size,
                        Color = productItemDto.Color.color,
                        ColorImage = productItemDto.Color.url,
                        QtyInStock = productItemDto.Qty
                    };
                    var imageUrls = await _cloudinaryService.UploadImagesAsync(productItemDto.Images);
                    foreach (var imageUrl in imageUrls)
                    {
                        productItem.ProductItemImages.Add(new ProductItemImage { Url = imageUrl });
                    }
                    product.ProductItems.Add(productItem);
                }
            }
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
