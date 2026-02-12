using System.ComponentModel.DataAnnotations;

namespace TraumaReflectionApp.Models.ViewModels
{
    public class ReflectionReplyViewModel
    {
        public int ReflectionID { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
    }
}