using backendtest.Data;
using backendtest.Dtos.CategoryDto;
using backendtest.Interfaces;
using backendtest.Models;
using Microsoft.EntityFrameworkCore;

namespace backendtest.Repository;

public class CategoryRepository : ICategoryRepository
{
   private readonly ApplicationContext _context;
    

   public CategoryRepository(ApplicationContext context)
   {
      _context = context;

   }
   public async Task<Category> CreateCategory(string categoryName)
   {
      if (await _context.Categories.AnyAsync(c => c.Name == categoryName))
         throw new ArgumentException("Category already exists");
      var category = new Category()
      {
         Name = categoryName,
      };
      await _context.Categories.AddAsync(category);
      await _context.SaveChangesAsync();
      return category;
   }

   public async Task<List<CategoryDto>> GetCategories()
   {
      return await _context.Categories
         .Select(c => new CategoryDto { Id = c.Id, Name = c.Name })
         .ToListAsync();
   }
   
   public async Task<CategoryDto> UpdateCategory(int id, string categoryName)
   {
      if (await _context.Categories.AnyAsync(c => c.Name == categoryName && c.Id != id))
         throw new ArgumentException("Category name already exists");
    
      var category = await _context.Categories.FindAsync(id);
      if (category == null)
         return null;

      category.Name = categoryName;
      _context.Categories.Update(category);
      await _context.SaveChangesAsync();

      return new CategoryDto { Id = category.Id, Name = category.Name }; // Возвращаем DTO
   }


   public async Task<bool> DeleteCategory(int id)
   {
      // Проверяем наличие связанных проектов
      bool hasProjects = await _context.Projects.AnyAsync(p => p.CategoryId == id);
      if (hasProjects)
      {
         throw new InvalidOperationException("Category cannot be deleted because it contains projects.");
      }

      // Если нет связанных проектов, удаляем категорию
      var category = await _context.Categories.FindAsync(id);
      if (category == null)
      {
         return false; // Категория не найдена
      }

      _context.Categories.Remove(category);
      await _context.SaveChangesAsync();
      return true;
   }

}