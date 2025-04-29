using Microsoft.EntityFrameworkCore;
using OrganizationManagement.DBContext;
using OrganizationManagement.Models;
using System.Linq;

namespace OrganizationManagement.Repository
{
    public class TestCaseRepository : ITestCaseRepository
    {
        private readonly ApplicationDbContext _context;

        public TestCaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public TestCase Add(TestCase testCase)
        {
            _context.TestCases.Add(testCase);
            _context.SaveChanges();
            return testCase;
        }

        public TestCase Update(TestCase testCase)
        {
            _context.TestCases.Update(testCase);
            _context.SaveChanges();
            return testCase;
        }

        public TestCase Delete(TestCase testCase)
        {
            _context.TestCases.Remove(testCase);
            _context.SaveChanges();
            return testCase;
        }

        public TestCase GetById(int id)
        {
            return _context.TestCases.FirstOrDefault(tc => tc.Id == id);
        }

        public bool Exists(int id)
        {
            return _context.TestCases.Any(e => e.Id == id);
        }
    }
}
