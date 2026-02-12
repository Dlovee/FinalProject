using System;

namespace TraumaReflectionApp.Models.ViewModels
{
    public class RecentReflectionDto
    {
        public int ReflectionID { get; set; }
        public string PreviewText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsPublic { get; set; }
    }
}