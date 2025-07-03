namespace StudyPlanner.Models;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ColorHex { get; set; }
    public List<StudySession> Sessions { get; set; } = new();
} 