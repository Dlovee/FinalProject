namespace TraumaReflectionApp.Repositories;
using System.Collections.Generic; 
using TraumaReflectionApp.Models;


public interface ICategoryRepository
{
    Category GetCategoryById(int id); 
    IEnumerable<Category> GetAllCategories(); 
    void AddCategory(Category category); 
    void UpdateCategory(Category category); 
    void DeleteCategory(int id);
}