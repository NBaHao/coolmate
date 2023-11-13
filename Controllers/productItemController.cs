using CoolMate.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Xml.Linq;
using WebApplication1.DTO;
using WebApplication1.Entities;
using WebApplication1.Filter;
using WebApplication1.Helpers;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class productItemController : ControllerBase
    {
        private readonly DBContext DBContext;
        private readonly UriService uriService;

        public productItemController(DBContext DBContext, UriService uriService)
        {
            this.DBContext = DBContext;
            this.uriService = uriService;
        }
        [HttpGet]
        public async Task<ActionResult<List<productItemDTO>>> Get([FromQuery] PaginationFilter filter)
        {
            var enpointUri = $"{this.Request.Scheme}://{this.Request.Host}/api/productItem";
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var query = from pi in DBContext.ProductItems
                        select new productItemDTO
                        {
                            ItemId = pi.Id,
                            ProductId = pi.ProductId,
                            Size = pi.Size,
                            Color = pi.Color,
                            ColorImage = pi.ColorImage,
                            QtyInStock = pi.QtyInStock,
                            CategoryId = pi.Product.CategoryId,
                            Name = pi.Product.Name,
                            Description = pi.Product.Description,
                            PriceInt = pi.Product.PriceInt,
                            PriceStr = pi.Product.PriceStr,
                        };

            var totalRecords = await query.CountAsync();
            var pagedData = await query
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<productItemDTO>(pagedData, validFilter, totalRecords, uriService, enpointUri);
            return Ok(pagedReponse);
        }
        [HttpGet("getAll")]
        public IActionResult Get()
        {
            return Ok(DBContext.ProductItems.Select(h => new { h.Id, h.Color, h.ColorImage, h.QtyInStock, h.Product }).ToList());
        }

        [HttpGet("getAll2")]
        public async Task<IActionResult> Get2()
        {
            var res = await DBContext.ProductItems.Select(h => new
            {
                h.Id,
                h.Color,
                h.ColorImage,
                h.QtyInStock,
                h.Product

            }).ToListAsync();
            return Ok(res);
        }
    }
}
