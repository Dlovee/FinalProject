using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TraumaReflectionApp.Models.ViewModels;
using TraumaReflectionApp.Repositories;

namespace TraumaReflectionApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReflectionRepository _reflectionRepo;
        private readonly IUserRepository _userRepo;

        public HomeController(
            IReflectionRepository reflectionRepo,
            IUserRepository userRepo)
        {
            _reflectionRepo = reflectionRepo;
            _userRepo = userRepo;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            // If not logged in, send new visitors to Register (not Login)
            if (userId == null)
            {
                return RedirectToAction("Register", "User");
            }

            var recentReflections = _reflectionRepo
                .GetReflectionsByUser(userId.Value)
                .Take(3)
                .Select(r => new RecentReflectionDto
                {
                    ReflectionID = r.ReflectionID,
                    PreviewText = string.IsNullOrEmpty(r.Content)
                        ? string.Empty
                        : (r.Content.Length > 80 ? r.Content.Substring(0, 80) + "…" : r.Content),
                    CreatedAt = r.CreatedAt,
                    IsPublic = r.IsPublic
                })
                .ToList();

            var user = _userRepo.GetById(userId.Value);

            var model = new TraumaReflectionApp.Models.ViewModels.HomeViewModel
            {
                Username = user.Username,
                RecentReflections = recentReflections
            };

            return View(model);
        }
    }
}