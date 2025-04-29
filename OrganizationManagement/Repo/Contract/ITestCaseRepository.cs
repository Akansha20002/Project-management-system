using OrganizationManagement.Models;

namespace OrganizationManagement.Repository
{
    public interface ITestCaseRepository
    {
        TestCase Add(TestCase testCase);
        TestCase Update(TestCase testCase);
        TestCase Delete(TestCase testCase);
        TestCase GetById(int id);
        bool Exists(int id);
    }
}
