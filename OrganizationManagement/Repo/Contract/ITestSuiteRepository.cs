using OrganizationManagement.Models;

namespace OrganizationManagement.Repo.Contract
{
    public interface ITestSuiteRepository
    {
        TestSuite GetById(int id);
        TestSuite Add(TestSuite testSuite);
        TestSuite Update(TestSuite testSuite);
        TestSuite Delete(int id);
        TestSuite GetByIdWithTestCases(int id);
    }
}
