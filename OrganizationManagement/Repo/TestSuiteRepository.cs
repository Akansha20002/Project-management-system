using Microsoft.EntityFrameworkCore;
using OrganizationManagement.DBContext;
using OrganizationManagement.Models;
using OrganizationManagement.Repo.Contract;

namespace OrganizationManagement.Repo
{
    public class TestSuiteRepository : ITestSuiteRepository
    {
        private readonly ApplicationDbContext _context;

        public TestSuiteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public TestSuite GetById(int id)
        {
            return _context.TestSuites.Find(id);
        }

        public TestSuite Add(TestSuite testSuite)
        {
            _context.TestSuites.Add(testSuite);
            _context.SaveChanges();
            return testSuite;
        }

        public TestSuite Update(TestSuite testSuite)
        {
            _context.TestSuites.Update(testSuite);
            _context.SaveChanges();
            return testSuite;
        }

        public TestSuite Delete(int id)
        {
            var testSuite = _context.TestSuites.Find(id);
            if (testSuite != null)
            {
                _context.TestSuites.Remove(testSuite);
                _context.SaveChanges();
            }
            return testSuite;
        }

        public TestSuite GetByIdWithTestCases(int id)
        {
            return _context.TestSuites
                .Include(ts => ts.TestCases)
                .FirstOrDefault(ts => ts.TestSuiteId == id);
        }
    }
}
