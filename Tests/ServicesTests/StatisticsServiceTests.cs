using StudyPlanner.Models;
using StudyPlanner.Services;
using StudyPlanner.Tests.Helpers;
using Xunit;

namespace StudyPlanner.Tests.ServicesTests
{
    public class StatisticsServiceTests : IDisposable
    {
        private readonly PlannerContext _context;
        private readonly StatisticsService _service;

        public StatisticsServiceTests()
        {
            _context = MockHelpers.CreateMockContext();
            _service = new StatisticsService(_context);
        }

        [Fact]
        public void GetTotalsBySubject_CalculatesHoursCorrectly()
        {
            var math = new Subject { Name = "Math" };
            var physics = new Subject { Name = "Physics" };
            _context.Subjects.AddRange(math, physics);
            _context.Sessions.AddRange(
                new StudySession { Subject = math, Start = DateTime.Now, End = DateTime.Now.AddHours(2) },
                new StudySession { Subject = math, Start = DateTime.Now, End = DateTime.Now.AddHours(3) },
                new StudySession { Subject = physics, Start = DateTime.Now, End = DateTime.Now.AddHours(1) }
            );
            _context.SaveChanges();

            var result = _service.GetTotalsBySubject(null).ToList();
            Assert.Equal(2, result.Count);
            Assert.Equal("Math", result[0].Subject);
            Assert.Equal(5, result[0].Hours);
            Assert.Equal("Physics", result[1].Subject);
            Assert.Equal(1, result[1].Hours);
        }

        [Fact]
        public void GetCurrentStreak_ReturnsCorrectStreak()
        {
            _context.Sessions.AddRange(
                new StudySession { Start = DateTime.Today, End = DateTime.Today.AddHours(1) },
                new StudySession { Start = DateTime.Today.AddDays(-1), End = DateTime.Today.AddDays(-1).AddHours(1) },
                new StudySession { Start = DateTime.Today.AddDays(-2), End = DateTime.Today.AddDays(-2).AddHours(1) }
            );
            _context.SaveChanges();

            var streak = _service.GetCurrentStreak();
            Assert.Equal(3, streak);
        }

        [Fact]
        public void GetCurrentStreak_ReturnsZeroWhenTodayMissing()
        {
            _context.Sessions.AddRange(
                new StudySession { Start = DateTime.Today.AddDays(-1), End = DateTime.Today.AddDays(-1).AddHours(1) },
                new StudySession { Start = DateTime.Today.AddDays(-2), End = DateTime.Today.AddDays(-2).AddHours(1) }
            );
            _context.SaveChanges();

            var streak = _service.GetCurrentStreak();
            Assert.Equal(0, streak);
        }

        public void Dispose() => _context.Dispose();
    }
}