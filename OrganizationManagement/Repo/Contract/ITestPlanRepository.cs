using OrganizationManagement.Models;

namespace OrganizationManagement.Repo.Contract
{
    public interface ITestPlanRepository
    {
        TestPlan Add(TestPlan testPlan);
        TestPlan Update(TestPlan testPlan);
        TestPlan Delete(TestPlan testPlan);
        TestPlan GetById(int id);
        TestPlan GetByIdWithTestSuites(int id);
        Project GetProjectById(int projectId);
        Project UpdateProject(Project project);
    }
}
