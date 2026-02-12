using Microsoft.AspNetCore.Mvc;
using TraumaReflectionApp.Repositories;

namespace TraumaReflectionApp.Controllers
{
    public class TraumaEntryController : Controller
    {
        private readonly ITraumaEntryRepository _traumaRepo;

        public TraumaEntryController(ITraumaEntryRepository traumaRepo)
        {
            _traumaRepo = traumaRepo;
        }

        public IActionResult Details(int id)
        {
            var entry = _traumaRepo.GetTraumaEntryById(id);
            return View(entry);
        }

        public IActionResult ByReflection(int reflectionId)
        {
            var entries = _traumaRepo.GetTraumaEntriesByReflection(reflectionId);
            return View(entries);
        }
    }
}