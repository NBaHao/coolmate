using CoolMate.DTO;
using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using CoolMate.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoolMate.Controllers
{
    [Route("api/[controller]")]
    public class categoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        public categoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> getAllCategory()
        {
            return Ok(await _categoryService.getAllCategory());
        }

        [HttpPost("add")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> addCategory([FromBody] AddCategoryDTO addcategoryDTO) {

            var res = await _categoryService.addCategory(addcategoryDTO);
            if (res) return Ok("successfully");
            return BadRequest(res);
        }

        [HttpPut("update")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> updateCategory([FromBody] UpdateCategoryDTO updatecategoryDTO)
        {
            var res = await _categoryService.updateCategory(updatecategoryDTO);
            if (res) return Ok("successfully");
            return BadRequest(res);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> deleteCategory([FromBody] int categoryId)
        {
            var res = await _categoryService.deleteCategory(categoryId);
            if (res) return Ok("successfully");
            return BadRequest(res);
        }
    }
}
