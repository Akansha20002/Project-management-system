using OrganizationManagement.Models;
using OrganizationManagement.Repo.Contract;
using OrganizationManagement.Services.Interface;

namespace OrganizationManagement.Services
{
    public class TestPlanService : ITestPlanService
    {

        public readonly ITestPlanRepository _repo;

        public TestPlanService(ITestPlanRepository repo)
        {
            _repo = repo;
        }

        public TestPlan Add(TestPlan testPlans)
        {
            return _repo.Add(testPlans);
        }

        public TestPlan Delete(TestPlan testPlans)
        {
            return _repo.Delete(testPlans);
        }

        public ICollection<TestPlan> GetTestPlansByUserId(string userId)
        {
            return _repo.GetTestPlansByUserId(userId);
        }

        public TestPlan Update(TestPlan testPlans)
        {
            return _repo.Update(testPlans);
        }
    }
}
