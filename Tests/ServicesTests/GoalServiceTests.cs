using StudyPlanner.Models;
using StudyPlanner.Services;
using StudyPlanner.Tests.Helpers;
using Xunit;

namespace StudyPlanner.Tests.ServicesTests
{
    public class GoalServiceTests : IDisposable
    {
        private readonly PlannerContext _context;
        private readonly GoalService _service;

        public GoalServiceTests()
        {
            _context = MockHelpers.CreateMockContext();
            _service = new GoalService(_context);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void AddGoal_ThrowsWhenTargetHoursInvalid(double hours)
        {
            _context.Subjects.Add(new Subject { Id = 1 });
            _context.SaveChanges();

            Assert.Throws<ArgumentException>(() => 
                _service.AddGoal(1, hours, DateTime.Today.AddDays(1)));
        }

        [Fact]
        public void AddGoal_ThrowsWhenDeadlineInPast()
        {
            _context.Subjects.Add(new Subject { Id = 1 });
            _context.SaveChanges();

            Assert.Throws<ArgumentException>(() => 
                _service.AddGoal(1, 10, DateTime.Today.AddDays(-1)));
        }

        [Fact]
        public void GetGoalProgress_ReturnsExpiredWhenDeadlinePassed()
        {
            _context.Subjects.Add(new Subject { Id = 1 });
            
            var goal = new Goal
            {
                SubjectId = 1,
                TargetHours = 10,
                Deadline = DateTime.Today.AddDays(-1)
            };
            
            _context.Goals.Add(goal);
            _context.SaveChanges();

            var result = _service.GetGoalProgress().First();
            Assert.Equal(GoalStatus.Expired, result.Status);
        }

        [Fact]
        public void GetGoalProgress_CalculatesProgressCorrectly()
        {
            // Arrange
            _context.Subjects.Add(new Subject { Id = 1 });
            _context.Sessions.Add(new StudySession
            {
                SubjectId = 1,
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(3)
            });
            var goal = new Goal
            {
                SubjectId = 1,
                TargetHours = 10,
                Deadline = DateTime.Today.AddDays(1)
            };
            _context.Goals.Add(goal);
            _context.SaveChanges();

            // Act
            var result = _service.GetGoalProgress().First();

            // Assert
            Assert.Equal(3, result.HoursDone);
            Assert.Equal(30, result.ProgressPct);
            
            Assert.Equal(GoalStatus.Warning, result.Status);
        }

        public void Dispose() => _context.Dispose();
    }
}