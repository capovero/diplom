// backendtest/Controllers/CategoryController.cs

using backendtest.Interfaces;
using backendtest.Models;
using backendtest.Dtos.CategoryDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backendtest.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto category)
        {
            if (category == null || string.IsNullOrWhiteSpace(category.Name))
                return BadRequest("Category name is required.");

            var newCategory = await _categoryRepository.CreateCategory(category.Name);
            return Ok(new CategoryDto { Id = newCategory.Id, Name = newCategory.Name });
        }

        [AllowAnonymous]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetCategories();
            return Ok(categories);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto category)
        {
            if (category == null || string.IsNullOrWhiteSpace(category.Name))
                return BadRequest("Category name is required.");

            var updatedCategory = await _categoryRepository.UpdateCategory(id, category.Name);
            if (updatedCategory == null)
                return NotFound("Category not found.");

            return Ok(updatedCategory);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var success = await _categoryRepository.DeleteCategory(id);
                if (!success)
                    return NotFound("Category not found.");

                return Ok("Category deleted successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
