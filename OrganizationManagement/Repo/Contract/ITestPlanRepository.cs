using OrganizationManagement.Models;

namespace OrganizationManagement.Repo.Contract
{
    public interface ITestPlanRepository
    {
        ICollection<TestPlan> GetTestPlansByUserId(string userId);
        TestPlan Add(TestPlan testPlans);
        TestPlan Update(TestPlan testPlans);

        TestPlan Delete(TestPlan testPlans);
        //TestSuite Add(TestSuite testSuites);
        //TestSuite Delete(TestSuite testSuites);
        //ICollection<TestSuite> GetTestPlansByUserId(int userId);
        //TestSuite Update(TestSuite testSuites);
    }
}
