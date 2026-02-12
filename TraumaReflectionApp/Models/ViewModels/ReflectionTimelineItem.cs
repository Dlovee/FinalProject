namespace TraumaReflectionApp.Models.ViewModels
{
    public class ReflectionTimelineItem
    {
        public int Id { get; set; }              // Maps from ReflectionID
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CategoryName { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}