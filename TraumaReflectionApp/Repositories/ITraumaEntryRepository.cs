using System.Collections.Generic;
using TraumaReflectionApp.Models;

namespace TraumaReflectionApp.Repositories
{
    public interface ITraumaEntryRepository
    {
        TraumaEntry GetTraumaEntryById(int id);
        IEnumerable<TraumaEntry> GetTraumaEntriesByUser(int userId);
        IEnumerable<TraumaEntry> GetTraumaEntriesByReflection(int reflectionId);
        void AddTraumaEntry(TraumaEntry entry);
        void UpdateTraumaEntry(TraumaEntry entry);
        void DeleteTraumaEntry(int id);
    }
}