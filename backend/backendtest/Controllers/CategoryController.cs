using backendtest.Data;
using backendtest.Interfaces;
using backendtest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backendtest.Controllers;
[Route("api/categories")]
[ApiController]

public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ApplicationContext _context;

    public CategoryController(ICategoryRepository categoryRepository, ApplicationContext context)
    {
        _categoryRepository = categoryRepository;
        _context = context;
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateCategory([FromBody] string category)
    {
        var newCategory = await _categoryRepository.CreateCategory(category);
        return Ok(newCategory);
    }

    [AllowAnonymous]
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryRepository.GetCategories();
        return Ok(categories);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateCategory([FromBody] string category, int id)
    {
        var updatedCategory = await _categoryRepository.UpdateCategory(id, category); // Ожидаем завершения задачи
        return Ok(updatedCategory); // Возвращаем результат
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            var success = await _categoryRepository.DeleteCategory(id);
            if (!success)
            {
                return NotFound("Category not found.");
            }
            return Ok("Category deleted successfully.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message); // Возвращаем сообщение об ошибке
        }
    }

}
