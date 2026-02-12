using Dapper;
using TraumaReflectionApp.Models;
using MySql.Data.MySqlClient;
using TraumaReflectionApp.Repositories;
using Microsoft.Extensions.Configuration;
public class AcknowledgmentRepository : IAcknowledgmentRepository
{
    private readonly string _connectionString;

    public AcknowledgmentRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
    }
    

    public void AddAcknowledgment(Acknowledgment acknowledgment)
    {
        using var connection = new MySqlConnection(_connectionString);

        var exists = connection.ExecuteScalar<int>(
            @"SELECT COUNT(*)
          FROM Acknowledgments
          WHERE ReflectionID = @ReflectionID
            AND UserID = @UserID
            AND Type = @Type",
            acknowledgment);

        if (exists > 0)
            return;

        connection.Execute(
            @"INSERT INTO Acknowledgments (ReflectionID, UserID, Type)
          VALUES (@ReflectionID, @UserID, @Type)",
            acknowledgment);
    }


    public int GetCountForReflection(int reflectionId, string type)
    {
        using var connection = new MySqlConnection(_connectionString);

        return connection.ExecuteScalar<int>(
            @"SELECT COUNT(*)
              FROM Acknowledgments
              WHERE ReflectionID = @reflectionId
                AND Type = @type",
            new { reflectionId, type });
    }

    public bool HasAcknowledged(int reflectionId, int userId, string type)
    {
        using var connection = new MySqlConnection(_connectionString);

        return connection.ExecuteScalar<int>(
            @"SELECT COUNT(*)
          FROM Acknowledgments
          WHERE ReflectionID = @reflectionId
            AND UserID = @userId
            AND Type = @type",
            new { reflectionId, userId, type }) > 0;
    }

    public Dictionary<string, int> GetCountsForReflection(int reflectionId)
    {
        using var connection = new MySqlConnection(_connectionString);

        var results = connection.Query(
            @"SELECT Type, COUNT(*) AS Count
          FROM Acknowledgments
          WHERE ReflectionID = @reflectionId
          GROUP BY Type",
            new { reflectionId });

        return results.ToDictionary(
            r => (string)r.Type,
            r => (int)r.Count
        );
    }

    public Dictionary<int, Dictionary<string, int>> GetCountsForAllReflections()
    {
        using var connection = new MySqlConnection(_connectionString);

        var results = connection.Query(
            @"SELECT ReflectionID, Type, COUNT(*) AS Count
          FROM Acknowledgments
          GROUP BY ReflectionID, Type");

        var dict = new Dictionary<int, Dictionary<string, int>>();

        foreach (var row in results)
        {
            int reflectionId = (int)row.ReflectionID;   // FIXED
            string type = row.Type;
            int count = (int)row.Count;                 // FIXED

            if (!dict.ContainsKey(reflectionId))
            {
                dict[reflectionId] = new Dictionary<string, int>
                {
                    { "Love", 0 },
                    { "Support", 0 },
                    { "Empathy", 0 }
                };
            }

            dict[reflectionId][type] = count;
        }

        return dict;
    }
    public void DeleteForReflection(int reflectionId)
    {
        using var connection = new MySqlConnection(_connectionString);

        connection.Execute(
            @"DELETE FROM Acknowledgments
          WHERE ReflectionID = @reflectionId",
            new { reflectionId });
    }

}