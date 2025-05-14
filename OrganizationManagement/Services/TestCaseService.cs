using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using OrganizationManagement.Repository;

namespace OrganizationManagement.Services
{
    public class TestCaseService : ITestCaseService
    {
        private readonly ITestCaseRepository _testCaseRepository;

        public TestCaseService(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }

        public TestCaseDTO CreateTestCase(TestCaseDTO model)
        {
            model.Steps = FormatSteps(model.Steps);
            var testCase = new TestCase
            {
                Title = model.Title,
                Description = model.Description,
                Steps = model.Steps,
                TestSuiteId = model.TestSuiteId,
                IsAutomated = model.IsAutomated
            };

            var createdTestCase = _testCaseRepository.Add(testCase);

            return new TestCaseDTO
            {
                Id = createdTestCase.Id,
                Title = createdTestCase.Title,
                Description = createdTestCase.Description,
                Steps = createdTestCase.Steps,
                TestSuiteId = createdTestCase.TestSuiteId,
                IsAutomated = createdTestCase.IsAutomated
            };
        }

        public TestCaseDTO UpdateTestCase(TestCaseDTO model)
        {
            var testCase = _testCaseRepository.GetById(model.Id);
            model.Steps = FormatSteps(model.Steps);
            if (testCase == null) return null;

            testCase.Title = model.Title;
            testCase.Description = model.Description;
            testCase.Steps = model.Steps;
            testCase.IsAutomated = model.IsAutomated;

            var updatedTestCase = _testCaseRepository.Update(testCase);

            return new TestCaseDTO
            {
                Id = updatedTestCase.Id,
                Title = updatedTestCase.Title,
                Description = updatedTestCase.Description,
                Steps = updatedTestCase.Steps,
                TestSuiteId = updatedTestCase.TestSuiteId,
                IsAutomated = updatedTestCase.IsAutomated
            };
        }

        public TestCaseDTO DeleteTestCase(int id)
        {
            var testCase = _testCaseRepository.GetById(id);
            if (testCase == null) return null;

            var deletedTestCase = _testCaseRepository.Delete(testCase);

            return new TestCaseDTO
            {
                Id = deletedTestCase.Id,
                Title = deletedTestCase.Title,
                Description = deletedTestCase.Description,
                Steps = deletedTestCase.Steps,
                TestSuiteId = deletedTestCase.TestSuiteId,
                IsAutomated = deletedTestCase.IsAutomated
            };
        }

        public TestCaseDTO GetTestCaseById(int id)
        {
            var testCase = _testCaseRepository.GetById(id);
            if (testCase == null) return null;

            return new TestCaseDTO
            {
                Id = testCase.Id,
                Title = testCase.Title,
                Description = testCase.Description,
                Steps = testCase.Steps,
                TestSuiteId = testCase.TestSuiteId,
                IsAutomated = testCase.IsAutomated
            };
        }
        private string FormatSteps(string steps)
        {
            if (string.IsNullOrWhiteSpace(steps))
                return steps;

            var lines = steps
                .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.All(line => line.Trim().StartsWith("Step ")))
            {
                return steps;
            }

            var formatted = lines
                .Select((line, index) => $"Step {index + 1}: {line.Trim()}")
                .ToArray();

            return string.Join(Environment.NewLine, formatted);
        }
        public bool TestCaseExists(int id)
        {
            return _testCaseRepository.Exists(id);
        }
    }
}
