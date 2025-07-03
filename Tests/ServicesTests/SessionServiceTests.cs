using StudyPlanner.Models;
using StudyPlanner.Services;
using StudyPlanner.Tests.Helpers;
using Xunit;

namespace StudyPlanner.Tests.ServicesTests
{
    public class SessionServiceTests : IDisposable
    {
        private readonly PlannerContext _context;
        private readonly SessionService _service;

        public SessionServiceTests()
        {
            _context = MockHelpers.CreateMockContext();
            _service = new SessionService(_context);
        }

        [Fact]
        public void AddSession_ThrowsWhenSubjectNotFound()
        {
            Assert.Throws<ArgumentException>(() => 
                _service.AddSession(1, DateTime.Now, DateTime.Now.AddHours(1), null));
        }

        [Fact]
        public void AddSession_SavesSessionWhenValid()
        {
            _context.Subjects.Add(new Subject { Id = 1 });
            _context.SaveChanges();

            var start = DateTime.Now;
            var end = start.AddHours(1);
            _service.AddSession(1, start, end, "Notes");

            var session = _context.Sessions.First();
            Assert.Equal(1, session.SubjectId);
            Assert.Equal(start, session.Start, TimeSpan.FromSeconds(1));
            Assert.Equal(end, session.End, TimeSpan.FromSeconds(1));
            Assert.Equal("Notes", session.Notes);
        }

        public void Dispose() => _context.Dispose();
    }
}