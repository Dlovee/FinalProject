using System;
using System.ComponentModel.DataAnnotations;

namespace TraumaReflectionApp.Models
{
    public class ReflectionReply
    {
        public int Id { get; set; }

        [Required]
        public int ReflectionID { get; set; }   // FIXED: must match Reflection model + SQL
        public Reflection Reflection { get; set; }

        [Required]
        public int UserId { get; set; }         // FIXED: was string UserID
        public User? User { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}