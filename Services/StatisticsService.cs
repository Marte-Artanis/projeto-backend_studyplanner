namespace StudyPlanner.Services;

using Microsoft.EntityFrameworkCore;
using System.Linq;

public class StatisticsService
{
    private readonly PlannerContext _db;
    public StatisticsService(PlannerContext db) => _db = db;

    public IEnumerable<SubjectHours> GetTotalsBySubject(DateTime? fromDate)
    {
        var query = _db.Sessions.AsQueryable();
        if (fromDate.HasValue)
            query = query.Where(s => s.Start.Date >= fromDate.Value.Date);

        return query.Include(s => s.Subject)
                    .AsEnumerable()
                    .GroupBy(s => s.Subject.Name)
                    .Select(g => new SubjectHours
                    {
                        Subject = g.Key,
                        Hours = Math.Round(g.Sum(x => (x.End - x.Start).TotalHours), 2)
                    })
                    .OrderByDescending(h => h.Hours);
    }

    public int GetCurrentStreak()
    {
        var studyDates = _db.Sessions.Select(s => s.Start.Date).Distinct().ToHashSet();
        int streak = 0;
        for (var d = DateTime.Today; studyDates.Contains(d); d = d.AddDays(-1))
            streak++;
        return streak;
    }
}

public class SubjectHours
{
    public string Subject { get; set; } = string.Empty;
    public double Hours { get; set; }
} 