using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTO;
using WebApplication1.Filter;
using WebApplication1.Helpers;
using WebApplication1.Services;
using WebApplication1.Wrappers;
using Microsoft.AspNetCore.Http;
using CoolMate.Models;

namespace CoolMate.Controllers.tmp
{
    [ApiController]
    [Route("api/[controller]")]
    public class productTestController : ControllerBase
    {
        private readonly DBContext DBContext;
        private readonly UriService uriService;

        public productTestController(DBContext DBContext, UriService uriService)
        {
            this.DBContext = DBContext;
            this.uriService = uriService;
        }
        [HttpGet]
        public async Task<ActionResult<List<productDTO>>> Get([FromQuery] PaginationFilter filter)
        {
            var enpointUri = $"{Request.Scheme}://{Request.Host}/api/product";
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var pagedData = await DBContext.Products.Select(
                s => new productDTO
                {
                    Id = s.Id,
                    CategoryId = s.CategoryId,
                    Name = s.Name,
                    Description = s.Description,
                    PriceInt = s.PriceInt,
                    PriceStr = s.PriceStr
                }
            ).Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToListAsync();

            var totalRecords = await DBContext.Products.CountAsync();

            //return Ok(new PagedResponse<List<productDTO>>(pagedData, validFilter.PageNumber, validFilter.PageSize));
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, uriService, enpointUri);
            return Ok(pagedReponse);
        }
    }
}
