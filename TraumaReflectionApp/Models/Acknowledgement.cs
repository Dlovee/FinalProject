namespace TraumaReflectionApp.Models;

public class Acknowledgment
{
    public int AcknowledgmentID { get; set; }
    public int ReflectionID { get; set; }
    public int UserID { get; set; }
    public string Type { get; set; }
    public DateTime CreatedAt { get; set; }
}