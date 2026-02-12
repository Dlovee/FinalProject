using TraumaReflectionApp.Models;
using TraumaReflectionApp.Models.ViewModels;

namespace TraumaReflectionApp.Repositories;

public interface IReflectionRepository
{
    IEnumerable<Reflection> GetReflectionsByUser(int userId);
    Reflection? GetReflectionById(int id);
    void AddReflection(Reflection reflection);
    void UpdateReflection(Reflection reflection);
    void DeleteReflection(int id);
    Reflection GetById(int reflectionId, int userId);
    void UpdateVisibility(Reflection reflection);

    
    IEnumerable<ReflectionTimelineItem> GetReflectionTimeline(int userId);
    IEnumerable<Reflection> GetPublicReflections();
    
}