using Microsoft.AspNetCore.Mvc;
using TraumaReflectionApp.Models;
using TraumaReflectionApp.Repositories;
using Microsoft.AspNetCore.Http;
using TraumaReflectionApp.Models.ViewModels;

namespace TraumaReflectionApp.Controllers;

public class ReflectionController : Controller
{
    private readonly IReflectionRepository _reflectionRepo;
    private readonly IAcknowledgmentRepository _ackRepo;
    private readonly IReflectionReplyRepository _replyRepo;

    public ReflectionController(
        IReflectionRepository reflectionRepo,
        IAcknowledgmentRepository ackRepo,
        IReflectionReplyRepository replyRepo)
    {
        _reflectionRepo = reflectionRepo;
        _ackRepo = ackRepo;       // FIXED
        _replyRepo = replyRepo;
    }
    
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Reflection reflection)
    {
        int? userId = HttpContext.Session.GetInt32("UserID");

        if (userId == null)
        {
            return RedirectToAction("Login", "User");
        }

        if (!ModelState.IsValid)
        {
            return View(reflection);
        }

        reflection.UserID = userId.Value;
        reflection.IsPublic = false;

        _reflectionRepo.AddReflection(reflection);
        return RedirectToAction("Index");
    }


    public IActionResult List()
    {
        int? userId = HttpContext.Session.GetInt32("UserID");
        if (userId == null)
            return RedirectToAction("Login", "User");

        var reflections = _reflectionRepo.GetReflectionsByUser(userId.Value);
        return View(reflections);
    }

    public IActionResult Details(int id)
    {
        var reflection = _reflectionRepo.GetReflectionById(id);

        if (reflection == null)
            return NotFound();

        var userId = HttpContext.Session.GetInt32("UserID");
        var isOwner = userId != null && reflection.UserID == userId.Value;

        // 🔒 Private reflections are only viewable by their owner
        if (!reflection.IsPublic && !isOwner)
            return NotFound();

        ViewBag.AckCounts = _ackRepo.GetCountsForReflection(id);

        return View(reflection);
    }


    public IActionResult Delete(int id)
    {
        _reflectionRepo.DeleteReflection(id);
        return RedirectToAction("Index");
    }
    
    public IActionResult Index()
    {
        int? userId = HttpContext.Session.GetInt32("UserID");

        if (userId == null)
        {
            return RedirectToAction("Login", "User");
        }

        var reflections = _reflectionRepo.GetReflectionsByUser(userId.Value);

        return View(reflections);
    }
    
    public IActionResult Edit(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserID");
        if (userId == null)
            return RedirectToAction("Login", "User");

        var reflection = _reflectionRepo.GetById(id, userId.Value);
        if (reflection == null)
            return NotFound();

        return View(reflection);
    }
    
    [HttpPost]
    public IActionResult Edit(Reflection reflection)
    {
        var userId = HttpContext.Session.GetInt32("UserID");
        if (userId == null)
            return RedirectToAction("Login", "User");

        reflection.UserID = userId.Value;

        _reflectionRepo.UpdateReflection(reflection);

        return RedirectToAction("Index");
    }
    
    [HttpPost]
    public IActionResult Share(int id)
    {
        int? userId = HttpContext.Session.GetInt32("UserID");

        if (userId == null)
            return RedirectToAction("Login", "User");

        var reflection = _reflectionRepo.GetReflectionById(id);

        if (reflection == null || reflection.UserID != userId.Value)
            return RedirectToAction("Index");

        reflection.IsPublic = true;
        _reflectionRepo.UpdateVisibility(reflection);

        return RedirectToAction("Forum");
    }
    
    [HttpPost]
    public IActionResult Acknowledge(int reflectionId, string type)
    {
        int? userId = HttpContext.Session.GetInt32("UserID");

        if (userId == null)
            return RedirectToAction("Login", "User");

        // 🚫 Prevent duplicate acknowledgments
        if (_ackRepo.HasAcknowledged(reflectionId, userId.Value, type))
            return RedirectToAction("Forum");

        var acknowledgment = new Acknowledgment
        {
            ReflectionID = reflectionId,
            UserID = userId.Value,
            Type = type
        };

        _ackRepo.AddAcknowledgment(acknowledgment);

        return RedirectToAction("Forum");
    }
    
    public IActionResult Forum()
    {
        var reflections = _reflectionRepo.GetPublicReflections();

        int? userId = HttpContext.Session.GetInt32("UserID");

        // Load counts for all reflections
        ViewBag.AckCounts = _ackRepo.GetCountsForAllReflections();

        // Load which reflections the user has acknowledged
        var userAckStatus = new Dictionary<int, HashSet<string>>();

        if (userId != null)
        {
            foreach (var reflection in reflections)
            {
                userAckStatus[reflection.ReflectionID] = new HashSet<string>();

                if (_ackRepo.HasAcknowledged(reflection.ReflectionID, userId.Value, "Love"))
                    userAckStatus[reflection.ReflectionID].Add("Love");

                if (_ackRepo.HasAcknowledged(reflection.ReflectionID, userId.Value, "Support"))
                    userAckStatus[reflection.ReflectionID].Add("Support");

                if (_ackRepo.HasAcknowledged(reflection.ReflectionID, userId.Value, "Empathy"))
                    userAckStatus[reflection.ReflectionID].Add("Empathy");
            }
        }

        ViewBag.UserAckStatus = userAckStatus;

        return View(reflections);
    }
    
    [HttpPost]
    public IActionResult Unshare(int id)
    {
        int? userId = HttpContext.Session.GetInt32("UserID");

        if (userId == null)
            return RedirectToAction("Login", "User");

        var reflection = _reflectionRepo.GetReflectionById(id);

        if (reflection == null || reflection.UserID != userId.Value)
            return RedirectToAction("Index");

        reflection.IsPublic = false;
        _reflectionRepo.UpdateVisibility(reflection);

        _ackRepo.DeleteForReflection(id);

        return RedirectToAction("Details", new { id });
    }
    public IActionResult ViewReflection(int id)
    {
        if (id == 0)
        {
            return NotFound();
        }

        var reflection = _reflectionRepo.GetReflectionById(id);

        if (reflection == null)
        {
            return NotFound();
        }

        var replies = _replyRepo.GetRepliesByReflectionId(id);
        ViewBag.Replies = replies;

        return View(reflection);
    }
    
    [HttpPost]
    public IActionResult CreateReply(ReflectionReply reply)
    {
        // Get the current logged-in user ID from session
        var currentUserId = HttpContext.Session.GetInt32("UserID");

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        // Load the reflection to check who wrote it
        var reflection = _reflectionRepo.GetReflectionById(reply.ReflectionID);

        if (reflection == null)
        {
            return NotFound();
        }

        // BLOCK replying to your own reflection
        if (reflection.UserID == currentUserId)
        {
            return Forbid(); // Hard stop — no self-replies allowed
        }

        // Normal reply creation
        reply.UserId = currentUserId.Value;
        reply.CreatedAt = DateTime.UtcNow;

        _replyRepo.AddReply(reply);

        return RedirectToAction("ViewReflection", new { id = reply.ReflectionID });
    }

}