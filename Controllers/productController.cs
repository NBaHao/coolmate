using AutoMapper;
using CloudinaryDotNet;
using CoolMate.DTO;
using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using CoolMate.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoolMate.Filter;
using CoolMate.Helpers;
using CoolMate.Services;
using Microsoft.Extensions.Caching.Memory;

namespace CoolMate.Controllers
{
    [Route("api/[controller]")]
    public class productController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly UriService _uriService;
        private readonly CloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public productController(
            IProductRepository productRepository,
            UriService uriService,
            CloudinaryService cloudinaryService,
            IMapper mapper,
            IMemoryCache memoryCache)
        {
            _productRepository = productRepository;
            _uriService = uriService;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var enpointUri = $"{Request.Scheme}://{Request.Host}/api/product";
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var products = await _productRepository.GetAllProducts();
            var totalRecords = products.Count();
            var data = products
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToList();

            var reponse = PaginationHelper.CreatePagedReponse(data, validFilter, totalRecords, _uriService, enpointUri);
            return Ok(reponse);
        }

        [HttpGet("get/{productId}")]
        public async Task<ProductDTO> GetProduct(int productId)
        {
            var product = await _productRepository.GetProductAsync(productId);
            return _mapper.Map<ProductDTO>(product);
        }

        [HttpGet("search")]
        public async Task<ActionResult> SearchProduct(string searchTerm)
        {
            var products = await _productRepository.SearchProductAsync(searchTerm);
            var productDTOs = products.Select(p => new ProductSearchDTO
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                Name = p.Name,
                Description = p.Description,
                PriceInt = p.PriceInt,
                PriceStr = p.PriceStr,
                Img = p.ProductItems.FirstOrDefault()?.ProductItemImages.FirstOrDefault()?.Url
            }).ToList();

            return Ok(productDTOs);

        }

        [HttpGet("{category}")]
        public async Task<ActionResult> GetByCategoryWithFilter(string category, [FromQuery] PaginationFilter paging, [FromQuery] string filter)
        {
            var enpointUri = $"{Request.Scheme}://{Request.Host}/api/product/{category}";
            var validFilter = new PaginationFilter(paging.PageNumber, paging.PageSize);
            string cacheKey = $"{category}-{filter}-{validFilter.PageNumber}-{validFilter.PageSize}";

            if (!_memoryCache.TryGetValue(cacheKey, out PagedResponse<List<ProductResDTO>> cacheEntry))
            {
                List<Product> productsWithItemsAndImages;

                if (filter == null)
                    productsWithItemsAndImages = await _productRepository.GetProductsByCategoryAsync(category);
                else
                    productsWithItemsAndImages = await _productRepository.GetProductsByCategoryWithFilterAsync(category, filter);

                var data = new List<ProductResDTO>();

                foreach (var product in productsWithItemsAndImages)
                {
                    var productData = new ProductResDTO
                    {
                        Id = product.Id,
                        CategoryId = product.CategoryId,
                        Name = product.Name,
                        Description = product.Description,
                        PriceInt = product.PriceInt,
                        PriceStr = product.PriceStr,
                        ProductItems = new List<ProductItemResDTO>()
                    };
                    foreach (var productItem in product.ProductItems)
                    {
                        var productItemData = new ProductItemResDTO
                        {
                            Id = productItem.Id,
                            Color = productItem.Color,
                            ColorImage = productItem.ColorImage,
                            Size = productItem.Size,
                            QtyInStock = productItem.QtyInStock,
                            ProductItemImages = productItem.ProductItemImages.Select(i => i.Url).ToList()
                        };

                        productItemData.Sizes = product.ProductItems
                            .Where(pi => pi.Color == productItem.Color && pi.QtyInStock > 0)
                            .Select(pi => pi.Size)
                            .ToList();

                        productItemData.ItemIds = product.ProductItems
                            .Where(pi => pi.Color == productItem.Color && pi.QtyInStock > 0)
                            .Select(pi => pi.Id)
                            .ToList();
                        productData.ProductItems.Add(productItemData);
                    }
                    data.Add(productData);
                }

                foreach (var product in data)
                {
                    // Select distinct items based on the "Color" property
                    var uniqueItems = product.ProductItems.GroupBy(i => i.Color).Select(group => group.First()).ToList();

                    product.ProductItems = uniqueItems;
                }

                var totalRecords = data.Count();

                var dataPaged = data
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize).ToList();

                var reponse = PaginationHelper.CreatePagedReponse(dataPaged, validFilter, totalRecords, _uriService, enpointUri);
                
                _memoryCache.Set(cacheKey, reponse, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }

            return Ok(_memoryCache.Get(cacheKey));
        }

        [Authorize(Roles = "admin")]
        [HttpPost("add")]
        public async Task<ActionResult> CreateProduct([FromForm] CreateProductDTO createProductDTO)
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
                List<string> imageUrls = null;
                foreach (var size in productItemDto.Size)
                {
                    var productItem = new ProductItem
                    {
                        Size = size,
                        Color = productItemDto.Color.color,
                        ColorImage = productItemDto.Color.url,
                        QtyInStock = productItemDto.Qty
                    };
                    if (imageUrls == null)
                        imageUrls = await _cloudinaryService.UploadImagesAsync(productItemDto.Images);
                    foreach (var imageUrl in imageUrls)
                    {
                        productItem.ProductItemImages.Add(new ProductItemImage { Url = imageUrl });
                    }
                    product.ProductItems.Add(productItem);
                }
            }
            await _productRepository.CreateProductAsync(product);
            return Ok("successfully");
        }
        [Authorize(Roles = "admin")]
        [HttpPut("update")]
        public async Task<ActionResult> UpdateProduct([FromBody] UpdateProductDTO updateProductDTO)
        {
            var res = await _productRepository.UpdateProductAsync(_mapper.Map<Product>(updateProductDTO));
            if (res) return Ok("successfully");
            return BadRequest(res);
        }
    }
}