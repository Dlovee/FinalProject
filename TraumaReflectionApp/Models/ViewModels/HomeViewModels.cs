using System.Collections.Generic;

namespace TraumaReflectionApp.Models.ViewModels
{
    public class HomeViewModel
    {
        public string Username { get; set; }
        public List<RecentReflectionDto> RecentReflections { get; set; } = new();
    }
}