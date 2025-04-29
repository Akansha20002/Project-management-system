using Microsoft.EntityFrameworkCore;
using OrganizationManagement.DBContext;
using OrganizationManagement.Models;
using OrganizationManagement.Repo.Contract;

namespace OrganizationManagement.Repo
{
    public class TestPlanRepository : ITestPlanRepository
    {
        private readonly ApplicationDbContext _context;

        public TestPlanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public TestPlan Add(TestPlan testPlan)
        {
            _context.TestsPlans.Add(testPlan);
            _context.SaveChanges();
            return testPlan;
        }

        public TestPlan Update(TestPlan testPlan)
        {
            _context.TestsPlans.Update(testPlan);
            _context.SaveChanges();
            return testPlan;
        }

        public TestPlan Delete(TestPlan testPlan)
        {
            _context.TestsPlans.Remove(testPlan);
            _context.SaveChanges();
            return testPlan;
        }

        public TestPlan GetById(int id)
        {
            return _context.TestsPlans.Find(id);
        }

        public TestPlan GetByIdWithTestSuites(int id)
        {
            return _context.TestsPlans
                .Include(tp => tp.TestSuites)
                .FirstOrDefault(tp => tp.TestPlanId == id);
        }

        public Project GetProjectById(int projectId)
        {
            return _context.Projects.FirstOrDefault(p => p.ProjectId == projectId);
        }

        public Project UpdateProject(Project project)
        {
            _context.Projects.Update(project);
            _context.SaveChanges();
            return project;
        }
    }
}

