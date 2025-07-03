namespace StudyPlanner.Models

{
    public class StudySession
    {
        public int Id { get; set; }         
        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = default!;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Duration => End - Start;
        public string? Notes { get; set; }
    }
}