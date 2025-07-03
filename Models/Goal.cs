namespace StudyPlanner.Models;

public enum GoalStatus
{
    OnTrack,
    Warning,
    Done,
    Expired
}

public class Goal
{
    public int Id { get; set; }
    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = default!;
    public double TargetHours { get; set; }
    public DateTime Deadline { get; set; }
} 