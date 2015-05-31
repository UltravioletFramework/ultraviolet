using System.Data.Entity;
using UvTestRunner.Models;

namespace UvTestRunner.Data
{
    public class TestRunContext : DbContext
    {
        public TestRunContext()
            : base("TestRunContext")
        {

        }

        public DbSet<TestRun> TestRuns { get; set; }
    }
}
