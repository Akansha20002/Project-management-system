using OrganizationManagement.DTO;

namespace OrganizationManagement.Services.Interface
{
    public interface ITestSuiteService
    {
        TestSuiteDTO GetTestSuiteById(int id);
        TestSuiteDTO CreateTestSuite(TestSuiteDTO dto);
        TestSuiteDTO UpdateTestSuite(TestSuiteDTO dto);
        bool DeleteTestSuite(int id);
    }
}
