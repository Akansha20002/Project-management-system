using OrganizationManagement.DBContext;
using OrganizationManagement.Models;
using OrganizationManagement.Repo.Contract;

namespace OrganizationManagement.Repo
{
    public class TestPlanRepository : ITestPlanRepository
    {
        public readonly ApplicationDbContext _context;

        public TestPlanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public TestPlan Add(TestPlan testPlans)
        {
            _context.TestsPlans.Add(testPlans);
            _context.SaveChanges();
            return testPlans;
            
        }

        public TestPlan Delete(TestPlan testPlans)
        {
            _context.TestsPlans.Remove(testPlans);
            _context.SaveChanges();
            return testPlans;
        }

        public ICollection<TestPlan> GetTestPlansByUserId(string userId)
        {
            return _context.TestsPlans
                          .Where(tp => tp.CreatedBy == userId)
                          .ToList();
        }

        public TestPlan Update(TestPlan testPlans)
        {
            _context.TestsPlans.Update(testPlans);
            _context.SaveChanges();
            return testPlans;
        }
    }
}
