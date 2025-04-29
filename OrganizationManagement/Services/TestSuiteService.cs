using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using OrganizationManagement.Repo.Contract;
using OrganizationManagement.Services.Interface;

namespace OrganizationManagement.Services
{
    public class TestSuiteService : ITestSuiteService
    {
        private readonly ITestSuiteRepository _repository;

        public TestSuiteService(ITestSuiteRepository repository)
        {
            _repository = repository;
        }

        public TestSuiteDTO GetTestSuiteById(int id)
        {
            var testSuite = _repository.GetByIdWithTestCases(id);
            if (testSuite == null)
                return null;

            return new TestSuiteDTO
            {
                TestSuiteId = testSuite.TestSuiteId,
                Name = testSuite.Name,
                Description = testSuite.Description,
                TestPlanId = testSuite.TestPlanId,
                TestCases = testSuite.TestCases
            };
        }

        public TestSuiteDTO CreateTestSuite(TestSuiteDTO dto)
        {
            var testSuite = new TestSuite
            {
                Name = dto.Name,
                Description = dto.Description,
                TestPlanId = dto.TestPlanId
            };

            var createdTestSuite = _repository.Add(testSuite);

            return new TestSuiteDTO
            {
                TestSuiteId = createdTestSuite.TestSuiteId,
                Name = createdTestSuite.Name,
                Description = createdTestSuite.Description,
                TestPlanId = createdTestSuite.TestPlanId
            };
        }

        public TestSuiteDTO UpdateTestSuite(TestSuiteDTO dto)
        {
            var testSuite = _repository.GetById(dto.TestSuiteId);
            if (testSuite == null)
                return null;

            testSuite.Name = dto.Name;
            testSuite.Description = dto.Description;

            var updatedTestSuite = _repository.Update(testSuite);

            return new TestSuiteDTO
            {
                TestSuiteId = updatedTestSuite.TestSuiteId,
                Name = updatedTestSuite.Name,
                Description = updatedTestSuite.Description,
                TestPlanId = updatedTestSuite.TestPlanId
            };
        }

        public bool DeleteTestSuite(int id)
        {
            var testSuite = _repository.Delete(id);
            return testSuite != null;
        }
    }
}

