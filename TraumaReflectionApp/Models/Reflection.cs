using System.ComponentModel.DataAnnotations;

namespace TraumaReflectionApp.Models;

public class Reflection
{
    public int ReflectionID { get; set; }
    public int UserID { get; set; }
    public int CategoryID { get; set; }

    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Reflection content is required")]
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    
    public bool IsPublic { get; set; } = false;
}