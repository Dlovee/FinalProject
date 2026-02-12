namespace TraumaReflectionApp.Repositories;

using TraumaReflectionApp.Models;

public interface IAcknowledgmentRepository
{
    void AddAcknowledgment(Acknowledgment acknowledgment);
    int GetCountForReflection(int reflectionId, string type);
    bool HasAcknowledged(int reflectionId, int userId, string type);
    Dictionary<string, int> GetCountsForReflection(int reflectionId);
    Dictionary<int, Dictionary<string, int>> GetCountsForAllReflections();
    void DeleteForReflection(int reflectionId);
}
