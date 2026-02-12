using Microsoft.AspNetCore.Mvc;
using TraumaReflectionApp.Repositories;

namespace TraumaReflectionApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public IActionResult Index()
        {
            var categories = _categoryRepo.GetAllCategories();
            return View(categories);
        }

        public IActionResult Details(int id)
        {
            var category = _categoryRepo.GetCategoryById(id);
            return View(category);
        }
    }
}