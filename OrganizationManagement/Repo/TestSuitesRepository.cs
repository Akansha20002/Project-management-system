using OrganizationManagement.DBContext;
using OrganizationManagement.Models;
using OrganizationManagement.Repo.Contract;

namespace OrganizationManagement.Repo
{
    public class TestSuitesRepository : ITestSuiteRepository
    {
        public readonly ApplicationDbContext _context;

        public TestSuitesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public TestSuite Add(TestSuite testSuites)
        {
            _context.TestSuites.Add(testSuites);
            _context.SaveChanges();
            return testSuites;

        }

        public TestSuite Delete(TestSuite testSuites)
        {
            _context.TestSuites.Remove(testSuites);
            _context.SaveChanges();
            return testSuites;
        }

        public ICollection<TestSuite> GetTestSuiteByUserId(int userId)
        {
            return _context.TestSuites
                         .Where(tp => tp.TestPlanId == userId)
                         .ToList();
        }

        public TestSuite Update(TestSuite testSuites)
        {
            _context.TestSuites.Update(testSuites);
            _context.SaveChanges();
            return testSuites;


        }
    }
}
