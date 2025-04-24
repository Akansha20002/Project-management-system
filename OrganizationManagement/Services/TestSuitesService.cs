using OrganizationManagement.Models;
using OrganizationManagement.Repo.Contract;
using OrganizationManagement.Services.Interface;

namespace OrganizationManagement.Services
{
    public class TestSuitesService:ITestSuitesService
    {
        public readonly ITestSuiteRepository _repo;

        public TestSuitesService(ITestSuiteRepository repo)
        {
            _repo = repo;
        }

        public TestSuite Add(TestSuite testSuites)
        {
            return _repo.Add(testSuites);
        }

        public TestSuite Delete(TestSuite testSuites)
        {
            return _repo.Delete(testSuites);
        }

      

        public ICollection<TestSuite> GetTestSuiteByUserId(int userId)
        {
            return _repo.GetTestSuiteByUserId(userId);
        }

        public TestSuite Update(TestSuite testSuites)
        {
            return _repo.Update(testSuites);
        }
    }
}
