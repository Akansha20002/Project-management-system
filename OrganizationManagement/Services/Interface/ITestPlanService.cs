using OrganizationManagement.DTO;

namespace OrganizationManagement.Services.Interface
{
    public interface ITestPlanService
    {
        TestPlanDTO AddTestPlan(TestPlanDTO dto);
        TestPlanDTO UpdateTestPlan(TestPlanDTO dto);
        bool DeleteTestPlan(int id);
        TestPlanDTO GetTestPlanById(int id);
        TestPlanDTO GetTestPlanDetails(int id);
    }
}