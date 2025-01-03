using backendtest.Dtos.CategoryDto;
using backendtest.Models;

namespace backendtest.Interfaces;

public interface ICategoryRepository
{
    Task<Category> CreateCategory(string categoryName);
    Task<List<CategoryDto>> GetCategories();
    Task<CategoryDto> UpdateCategory(int id, string categoryName);
    Task<bool> DeleteCategory(int id);
}