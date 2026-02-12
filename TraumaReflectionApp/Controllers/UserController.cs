using Microsoft.AspNetCore.Mvc;
using TraumaReflectionApp.Models;
using TraumaReflectionApp.Repositories;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;


namespace TraumaReflectionApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        // GET: /User/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /User/Register
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            _userRepo.AddUser(user);
            return RedirectToAction("Login");
        }


        // GET: /User/Login
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserID") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: /User/Login
        [HttpPost]
        public IActionResult Login(User user)
        {
            var existingUser = _userRepo.GetUserByUsername(user.Username);

            if (existingUser == null || existingUser.PasswordHash != user.PasswordHash)
            {
                ViewBag.Error = "Invalid username or password";
                return View(user);
            }

            HttpContext.Session.SetInt32("UserID", existingUser.UserID);

            return RedirectToAction("Index", "Home");
        }


        // GET: /User/Profile
        public IActionResult Profile()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
                return RedirectToAction("Login");

            var user = _userRepo.GetById(userId.Value);
            return View(user);
        }


        // GET: /User/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}