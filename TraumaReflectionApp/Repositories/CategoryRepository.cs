using System.Collections.Generic;
using System.Data;
using Dapper;
using TraumaReflectionApp.Models;
using TraumaReflectionApp.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IDbConnection _connection;

    public CategoryRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public Category GetCategoryById(int id)
    {
        return _connection.QuerySingleOrDefault<Category>(
            "SELECT * FROM Categories WHERE CategoryID = @id;",
            new { id });
    }

    public IEnumerable<Category> GetAllCategories()
    {
        return _connection.Query<Category>("SELECT * FROM Categories;");
    }

    public void AddCategory(Category category)
    {
        _connection.Execute(
            "INSERT INTO Categories (Name) VALUES (@Name);",
            category);
    }

    public void UpdateCategory(Category category)
    {
        _connection.Execute(
            @"UPDATE Categories
              SET Name = @Name
              WHERE CategoryID = @CategoryID;",
            category);
    }

    public void DeleteCategory(int id)
    {
        _connection.Execute(
            "DELETE FROM Categories WHERE CategoryID = @id;",
            new { id });
    }
}