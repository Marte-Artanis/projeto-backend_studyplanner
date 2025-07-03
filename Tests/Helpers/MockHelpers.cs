using Microsoft.EntityFrameworkCore;
using StudyPlanner;
using System;

namespace StudyPlanner.Tests.Helpers
{
    public static class MockHelpers
    {
        public static PlannerContext CreateMockContext()
        {
            var options = new DbContextOptionsBuilder<PlannerContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new PlannerContext(options);
        }
    }
}