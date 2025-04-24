using OrganizationManagement.Models;

namespace OrganizationManagement.Repo.Contract
{
    public interface ITestSuiteRepository
    {
        ICollection<TestSuite> GetTestSuiteByUserId(int userId);
        TestSuite Add(TestSuite testSuites);
        TestSuite Update(TestSuite testSuites);

        TestSuite Delete(TestSuite testSuites);

    }
}
