namespace StudyPlanner.Services;

using Microsoft.EntityFrameworkCore;
using StudyPlanner.Models;
using System.Linq;

public class GoalService
{
    private readonly PlannerContext _db;
    public GoalService(PlannerContext db) => _db = db;

    public void AddGoal(int subjectId, double targetHours, DateTime deadline)
    {
        if (!_db.Subjects.Any(s => s.Id == subjectId))
            throw new ArgumentException("Matéria não encontrada.");
        if (targetHours <= 0)
            throw new ArgumentException("Horas alvo deve ser > 0.");
        if (deadline.Date < DateTime.Today)
            throw new ArgumentException("Deadline deve ser hoje ou futura.");

        _db.Goals.Add(new Goal
        {
            SubjectId = subjectId,
            TargetHours = targetHours,
            Deadline = deadline.Date
        });
        _db.SaveChanges();
    }

    public IEnumerable<GoalProgress> GetGoalProgress()
    {
        var goals = _db.Goals.Include(g => g.Subject).ToList();
        foreach (var g in goals)
        {
            var hoursDone = _db.Sessions
                .Where(s => s.SubjectId == g.SubjectId && s.End <= g.Deadline)
                .Sum(s => (double?)(s.End - s.Start).TotalHours) ?? 0; 
            var pct = g.TargetHours == 0 ? 0 : hoursDone / g.TargetHours;
            var status = GoalStatus.OnTrack;
            if (pct >= 1)
                status = GoalStatus.Done;
            else if (DateTime.Today > g.Deadline)
                status = GoalStatus.Expired;
            else if (pct < 0.5)
                status = GoalStatus.Warning;
            yield return new GoalProgress
            {
                Goal = g,
                HoursDone = Math.Round(hoursDone, 2),
                ProgressPct = Math.Round(pct * 100, 2),
                Status = status
            };
        }
    }
}

public class GoalProgress
{
    public Goal Goal { get; set; } = default!;
    public double HoursDone { get; set; }
    public double ProgressPct { get; set; }
    public GoalStatus Status { get; set; }
} 