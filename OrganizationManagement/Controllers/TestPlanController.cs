using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.DTO;
using OrganizationManagement.Services.Interface;

namespace OrganizationManagement.Controllers
{
    public class TestPlanController : Controller
    {
        private readonly ITestPlanService _testPlanService;

        public TestPlanController(ITestPlanService testPlanService)
        {
            _testPlanService = testPlanService;
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
                _testPlanService.AddTestPlan(dto);
                return RedirectToAction("ProjectDashboard", "Project", new { projectId = dto.ProjectId });
            }

            return View(dto);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var dto = _testPlanService.GetTestPlanById(id);
            if (dto == null)
                return NotFound();

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
                var result = _testPlanService.UpdateTestPlan(dto);
                if (result == null)
                    return NotFound();

                return RedirectToAction("ProjectDashboard", "Project", new { projectId = dto.ProjectId });
            }

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var testPlan = _testPlanService.GetTestPlanById(id);
            if (testPlan == null)
                return NotFound();

            var success = _testPlanService.DeleteTestPlan(id);
            if (!success)
                return NotFound();

            return RedirectToAction("ProjectDashboard", "Project", new { projectId = testPlan.ProjectId });
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var dto = _testPlanService.GetTestPlanDetails(id);
            if (dto == null)
                return NotFound();

            return View(dto);
        }
    }
}

