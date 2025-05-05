using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.DTO;
using OrganizationManagement.Services.Interface;

namespace OrganizationManagement.Controllers
{
    public class TestSuiteController : Controller
    {
        private readonly ITestSuiteService _testSuiteService;

        public TestSuiteController(ITestSuiteService testSuiteService)
        {
            _testSuiteService = testSuiteService;
        }

        // GET: TestSuite/Details   
        public IActionResult Details(int id)
        {
            var dto = _testSuiteService.GetTestSuiteById(id);
            if (dto == null)
            {
                return NotFound();
            }

            return View(dto); // Return TestSuite details with associated TestCases
        }

        // GET: TestSuite/Create
        public IActionResult Create(int testPlanId)
        {
            var model = new TestSuiteDTO { TestPlanId = testPlanId };
            return View(model);
        }

        // POST: TestSuite/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TestSuiteDTO model)
        {
            if (ModelState.IsValid)
            {
                var createdTestSuite = _testSuiteService.CreateTestSuite(model);
                return RedirectToAction("Details", "TestPlan", new { id = createdTestSuite.TestPlanId });
            }

            return View(model);
        }

        // GET: TestSuite/Edit/5
        public IActionResult Edit(int id)
        {
            var dto = _testSuiteService.GetTestSuiteById(id);
            if (dto == null)
            {
                return NotFound();
            }

            return View(dto);
        }

        // POST: TestSuite/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TestSuiteDTO model)
        {
            if (id != model.TestSuiteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updatedTestSuite = _testSuiteService.UpdateTestSuite(model);
                if (updatedTestSuite == null)
                {
                    return NotFound();
                }

                return RedirectToAction("Details", "TestPlan", new { id = updatedTestSuite.TestPlanId });
            }

            return View(model);
        }

        // GET: TestSuite/Delete/5
        public IActionResult Delete(int id)
        {
            var dto = _testSuiteService.GetTestSuiteById(id);
            if (dto == null)
            {
                return NotFound();
            }

            return View(dto);
        }

        // POST: TestSuite/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var success = _testSuiteService.DeleteTestSuite(id);
            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction("Details", "TestPlan", new { id = id });
        }
    }
}
