using StudyPlanner.Models;
using StudyPlanner.Services;
using StudyPlanner.Tests.Helpers;
using Xunit;

namespace StudyPlanner.Tests.ServicesTests
{
    public class SubjectServiceTests : IDisposable
    {
        private readonly PlannerContext _context;
        private readonly SubjectService _service;

        public SubjectServiceTests()
        {
            _context = MockHelpers.CreateMockContext();
            _service = new SubjectService(_context);
        }

        [Fact]
        public void Add_ThrowsWhenNameIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => _service.Add(""));
        }

        [Fact]
        public void Add_ThrowsWhenNameExists()
        {
            _context.Subjects.Add(new Subject { Name = "Math" });
            _context.SaveChanges();

            Assert.Throws<ArgumentException>(() => _service.Add("Math"));
        }

        [Fact]
        public void Add_SavesSubjectWhenValid()
        {
            _service.Add("Physics");
            Assert.Single(_context.Subjects);
            Assert.Equal("Physics", _context.Subjects.First().Name);
        }

        public void Dispose() => _context.Dispose();
    }
}