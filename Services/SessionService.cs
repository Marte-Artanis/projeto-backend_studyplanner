namespace StudyPlanner.Services;

using Microsoft.EntityFrameworkCore;
using StudyPlanner.Models;
using System.Linq;

public class SessionService
{
    private readonly PlannerContext _db;
    public SessionService(PlannerContext db) => _db = db;

    public void AddSession(int subjectId, DateTime start, DateTime end, string? notes)
    {
        if (!_db.Subjects.Any(s => s.Id == subjectId))
            throw new ArgumentException("Matéria não encontrada.");

        _db.Sessions.Add(new StudySession
        {
            SubjectId = subjectId,
            Start = start,
            End = end,
            Notes = string.IsNullOrWhiteSpace(notes) ? null : notes
        });
        _db.SaveChanges();
    }

    public IQueryable<StudySession> GetSessionsWithSubject() =>
        _db.Sessions.Include(s => s.Subject);
} 