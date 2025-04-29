using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using OrganizationManagement.Repo.Contract;
using OrganizationManagement.Services.Interface;

namespace OrganizationManagement.Services
{
    public class TestPlanService : ITestPlanService
    {
        private readonly ITestPlanRepository _repository;

        public TestPlanService(ITestPlanRepository repository)
        {
            _repository = repository;
        }

        public TestPlanDTO AddTestPlan(TestPlanDTO dto)
        {
            var testPlan = new TestPlan
            {
                Name = dto.Name.Trim(),
                Objective = dto.Objective,
                CreatedBy = dto.CreatedBy,
                Strategy = dto.Strategy,
                ProjectId = dto.ProjectId
            };

            var project = _repository.GetProjectById(dto.ProjectId);
            if (project != null && project.Status != "In Progress")
            {
                project.Status = "In Progress";
                _repository.UpdateProject(project);
            }

            var saved = _repository.Add(testPlan);
            dto.TestPlanId = saved.TestPlanId;
            return dto;
        }

        public TestPlanDTO UpdateTestPlan(TestPlanDTO dto)
        {
            var testPlan = _repository.GetById(dto.TestPlanId);
            if (testPlan == null)
                return null;

            testPlan.Name = dto.Name.Trim();
            testPlan.Objective = dto.Objective;
            testPlan.CreatedBy = dto.CreatedBy;
            testPlan.Strategy = dto.Strategy;

            _repository.Update(testPlan);

            return dto;
        }

        public bool DeleteTestPlan(int id)
        {
            var testPlan = _repository.GetById(id);
            if (testPlan == null)
                return false;

            _repository.Delete(testPlan);
            return true;
        }

        public TestPlanDTO GetTestPlanById(int id)
        {
            var testPlan = _repository.GetById(id);
            if (testPlan == null) return null;

            return new TestPlanDTO
            {
                TestPlanId = testPlan.TestPlanId,
                Name = testPlan.Name,
                Objective = testPlan.Objective,
                CreatedBy = testPlan.CreatedBy,
                Strategy = testPlan.Strategy,
                ProjectId = testPlan.ProjectId
            };
        }

        public TestPlanDTO GetTestPlanDetails(int id)
        {
            var testPlan = _repository.GetByIdWithTestSuites(id);
            if (testPlan == null) return null;

            return new TestPlanDTO
            {
                TestPlanId = testPlan.TestPlanId,
                Name = testPlan.Name,
                Objective = testPlan.Objective,
                CreatedBy = testPlan.CreatedBy,
                Strategy = testPlan.Strategy,
                ProjectId = testPlan.ProjectId,
                TestSuites = testPlan.TestSuites?.ToList()
            };
        }
    }
}

