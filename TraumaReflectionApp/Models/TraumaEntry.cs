namespace TraumaReflectionApp.Models;

public class TraumaEntry
{
    public int TraumaEntryID { get; set; } 
    public int UserID { get; set; } 
    public int ReflectionID { get; set; } 
    public string Title { get; set; } 
    public string Details { get; set; } 
    public DateTime CreatedAt { get; set; }
}