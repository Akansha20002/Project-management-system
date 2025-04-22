using OrganizationManagement.Models;

namespace OrganizationManagement.Services.Interface
{
    public interface ITestPlanService
    {

        ICollection<TestPlan> GetTestPlansByUserId(string userId);
        TestPlan Add(TestPlan testPlans);
        TestPlan Update(TestPlan testPlans);

        TestPlan Delete(TestPlan testPlans);

    }
}
