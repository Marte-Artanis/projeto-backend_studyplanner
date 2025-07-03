namespace StudyPlanner.Services;

using StudyPlanner.Models;
using System.Linq;

public class SubjectService
{
    private readonly PlannerContext _db;
    public SubjectService(PlannerContext db) => _db = db;

    public void Add(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome inválido.");

        if (_db.Subjects.Any(s => s.Name == name))
            throw new ArgumentException("Já existe matéria com esse nome.");

        _db.Subjects.Add(new Subject { Name = name });
        _db.SaveChanges();
    }

    public IEnumerable<Subject> GetAll() => _db.Subjects.OrderBy(s => s.Id);
} 