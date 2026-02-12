using System.Data;
using Dapper;
using TraumaReflectionApp.Models;

public class ReflectionReplyRepository : IReflectionReplyRepository
{
    private readonly IDbConnection _connection;

    public ReflectionReplyRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public void AddReply(ReflectionReply reply)
    {
        string sql = @"
            INSERT INTO ReflectionReplies (ReflectionId, UserId, Content, CreatedAt)
            VALUES (@ReflectionId, @UserId, @Content, @CreatedAt);";

        _connection.Execute(sql, reply);
    }

    public IEnumerable<ReflectionReply> GetRepliesByReflectionId(int reflectionId)
    {
        string sql = @"
        SELECT *
        FROM ReflectionReplies
        WHERE ReflectionId = @ReflectionId
        ORDER BY CreatedAt DESC;";

        return _connection.Query<ReflectionReply>(sql, new { ReflectionId = reflectionId });
    }
}