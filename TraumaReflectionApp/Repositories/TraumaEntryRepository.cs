using System.Collections.Generic;
using System.Data;
using Dapper;
using TraumaReflectionApp.Models;

namespace TraumaReflectionApp.Repositories
{
    public class TraumaEntryRepository : ITraumaEntryRepository
    {
        private readonly IDbConnection _connection;

        public TraumaEntryRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public TraumaEntry GetTraumaEntryById(int id)
        {
            return _connection.QuerySingleOrDefault<TraumaEntry>(
                "SELECT * FROM TraumaEntries WHERE TraumaEntryID = @id;",
                new { id });
        }

        public IEnumerable<TraumaEntry> GetTraumaEntriesByUser(int userId)
        {
            return _connection.Query<TraumaEntry>(
                "SELECT * FROM TraumaEntries WHERE UserID = @userId;",
                new { userId });
        }

        public IEnumerable<TraumaEntry> GetTraumaEntriesByReflection(int reflectionId)
        {
            return _connection.Query<TraumaEntry>(
                "SELECT * FROM TraumaEntries WHERE ReflectionID = @reflectionId;",
                new { reflectionId });
        }

        public void AddTraumaEntry(TraumaEntry entry)
        {
            _connection.Execute(
                @"INSERT INTO TraumaEntries (UserID, ReflectionID, Title, Details, CreatedAt)
                  VALUES (@UserID, @ReflectionID, @Title, @Details, @CreatedAt);",
                entry);
        }

        public void UpdateTraumaEntry(TraumaEntry entry)
        {
            _connection.Execute(
                @"UPDATE TraumaEntries
                  SET UserID = @UserID,
                      ReflectionID = @ReflectionID,
                      Title = @Title,
                      Details = @Details
                  WHERE TraumaEntryID = @TraumaEntryID;",
                entry);
        }

        public void DeleteTraumaEntry(int id)
        {
            _connection.Execute(
                "DELETE FROM TraumaEntries WHERE TraumaEntryID = @id;",
                new { id });
        }
    }
}
