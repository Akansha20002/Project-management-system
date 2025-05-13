using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizationManagement.DBContext;
using OrganizationManagement.DTO;
using OrganizationManagement.Models;

namespace OrganizationManagement.Controllers
{
    public class TestPlanController : Controller
    {
        private readonly ApplicationDbContext _tables;

        public TestPlanController(ApplicationDbContext tables)
        {
            _tables = tables;
        }

        [HttpGet]
        public IActionResult Add(int projectId)
        {
            return View(new TestPlanDTO { ProjectId = projectId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(TestPlanDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                ModelState.AddModelError("Name", "Test plan name is required.");

            if (string.IsNullOrWhiteSpace(dto.Objective))
                ModelState.AddModelError("Objective", "Objective is required.");

            if (ModelState.IsValid)
            {
                var testPlan = new TestPlan
                {
                    Name = dto.Name.Trim(),
                    Objective = dto.Objective,
                    CreatedBy = dto.CreatedBy,
                    Strategy = dto.Strategy,
                    ProjectId = dto.ProjectId
                };

                _tables.TestsPlans.Add(testPlan);

                var project = _tables.Projects.FirstOrDefault(p => p.ProjectId == dto.ProjectId);
                if (project != null && project.Status != "In Progress")
                {
                    project.Status = "In Progress";
                }

                _tables.SaveChanges();

                return RedirectToAction("ProjectDashboard", "Project", new { projectId = dto.ProjectId });
            }

            return View(dto);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var testPlan = _tables.TestsPlans.Find(id);
            if (testPlan == null)
                return NotFound();

            var dto = new TestPlanDTO
            {
                TestPlanId = testPlan.TestPlanId,
                Name = testPlan.Name,
                Objective = testPlan.Objective,
                CreatedBy = testPlan.CreatedBy,
                Strategy = testPlan.Strategy,
                ProjectId = testPlan.ProjectId
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TestPlanDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                ModelState.AddModelError("Name", "Test plan name is required.");

            if (string.IsNullOrWhiteSpace(dto.Objective))
                ModelState.AddModelError("Objective", "Objective is required.");

            if (ModelState.IsValid)
            {
                var testPlan = _tables.TestsPlans.Find(dto.TestPlanId);
                if (testPlan == null)
                    return NotFound();

                testPlan.Name = dto.Name.Trim();
                testPlan.Objective = dto.Objective;
                testPlan.CreatedBy = dto.CreatedBy;
                testPlan.Strategy = dto.Strategy;

                _tables.SaveChanges();

                return RedirectToAction("ProjectDashboard", "Project", new { projectId = testPlan.ProjectId });
            }

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var testPlan = _tables.TestsPlans.Find(id);
            if (testPlan == null)
                return NotFound();

            int projectId = testPlan.ProjectId;
            _tables.TestsPlans.Remove(testPlan);
            _tables.SaveChanges();

            return RedirectToAction("ProjectDashboard", "Project", new { projectId });
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var testPlan = _tables.TestsPlans
                .Include(tp => tp.TestSuites) // 💥 Includes test suites
                .FirstOrDefault(tp => tp.TestPlanId == id);

            if (testPlan == null)
                return NotFound();

            var dto = new TestPlanDTO
            {
                TestPlanId = testPlan.TestPlanId,
                Name = testPlan.Name,
                Objective = testPlan.Objective,
                CreatedBy = testPlan.CreatedBy,
                Strategy = testPlan.Strategy,
                ProjectId = testPlan.ProjectId,
                TestSuites = testPlan.TestSuites?.ToList()
            };

            return View(dto);
        }
    }
}
