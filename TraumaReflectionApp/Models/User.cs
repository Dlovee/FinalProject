using System.ComponentModel.DataAnnotations;

namespace TraumaReflectionApp.Models;

public class User
{
    public int UserID { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string PasswordHash { get; set; }
    public string? FirstName { get; set; }
    public string LastName { get; set; }

}