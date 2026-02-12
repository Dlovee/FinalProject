using System.Collections.Generic;
using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using TraumaReflectionApp.Models;
using TraumaReflectionApp.Models.ViewModels;

namespace TraumaReflectionApp.Repositories;

public class ReflectionRepository : IReflectionRepository
{
    private readonly IDbConnection _connection;

    public ReflectionRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public IEnumerable<Reflection> GetReflectionsByUser(int userId)
    {
        return _connection.Query<Reflection>(
            "SELECT * FROM Reflections WHERE UserID = @userId;",
            new { userId });
    }

    public Reflection? GetReflectionById(int id)
    {
        return _connection.QuerySingleOrDefault<Reflection>(
            "SELECT * FROM Reflections WHERE ReflectionID = @id;",
            new { id });
    }

    public void AddReflection(Reflection reflection)
    {
        _connection.Execute(
            @"INSERT INTO Reflections
              (UserID, CategoryID, Title, Content)
              VALUES
              (@UserID, @CategoryID, @Title, @Content);",
            reflection);
    }

    public void UpdateReflection(Reflection reflection)
    {
        _connection.Execute(
            @"UPDATE Reflections
              SET Title = @Title,
                  Content = @Content,
                  CategoryID = @CategoryID
              WHERE ReflectionID = @ReflectionID;",
            reflection);
    }

    public void DeleteReflection(int id)
    {
        _connection.Execute(
            "DELETE FROM Reflections WHERE ReflectionID = @id;",
            new { id });
    }

    public Reflection GetById(int reflectionId, int userId)
    {
        return _connection.QueryFirstOrDefault<Reflection>(
            @"SELECT *
          FROM Reflections
          WHERE ReflectionID = @reflectionId
            AND UserID = @userId;",
            new { reflectionId, userId });
    }



    public IEnumerable<ReflectionTimelineItem> GetReflectionTimeline(int userId)
    {
        return _connection.Query<ReflectionTimelineItem>(
            @"SELECT 
              r.ReflectionID AS Id,
              r.Title,
              r.Content,
              r.CreatedAt,
              c.Name AS CategoryName
          FROM Reflections r
          JOIN Categories c ON r.CategoryID = c.CategoryID
          WHERE r.UserID = @UserID
          ORDER BY r.CreatedAt DESC;",
            new { UserID = userId });
    }
    
    public void UpdateVisibility(Reflection reflection)
    {
        string sql = @"
        UPDATE Reflections
        SET IsPublic = @IsPublic
        WHERE ReflectionID = @ReflectionID";

        _connection.Execute(sql, reflection);
    }
    
    public IEnumerable<Reflection> GetPublicReflections()
    {
        string sql = @"
        SELECT *
        FROM Reflections
        WHERE IsPublic = TRUE
        ORDER BY CreatedAt DESC";

        return _connection.Query<Reflection>(sql);
    }
    
}
