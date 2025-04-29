using OrganizationManagement.DTO;
using OrganizationManagement.Models;

namespace OrganizationManagement.Services
{
    public interface ITestCaseService
    {
        TestCaseDTO CreateTestCase(TestCaseDTO model);
        TestCaseDTO UpdateTestCase(TestCaseDTO model);
        TestCaseDTO DeleteTestCase(int id);
        TestCaseDTO GetTestCaseById(int id);
        bool TestCaseExists(int id);
    }
}
