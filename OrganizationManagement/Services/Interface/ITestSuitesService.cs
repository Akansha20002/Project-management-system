using OrganizationManagement.Models;

namespace OrganizationManagement.Services.Interface
{
    public interface ITestSuitesService
    {
        ICollection<TestSuite> GetTestSuiteByUserId(int userId);
        TestSuite Add(TestSuite testSuites);
        TestSuite Update(TestSuite testSuites);

        TestSuite Delete(TestSuite testSuites);
    }
}
