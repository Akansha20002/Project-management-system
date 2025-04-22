using OrganizationManagement.Models;

namespace OrganizationManagement.Repo.Contract
{
    public interface ITestPlanRepository
    {
        ICollection<TestPlan> GetTestPlansByUserId(string userId);
        TestPlan Add(TestPlan testPlans);
        TestPlan Update(TestPlan testPlans);

        TestPlan Delete(TestPlan testPlans);


    }
}
