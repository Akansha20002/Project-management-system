using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.DTO;
using OrganizationManagement.Services;

public class TestCaseController : Controller
{
    private readonly ITestCaseService _testCaseService;

    public TestCaseController(ITestCaseService testCaseService)
    {
        _testCaseService = testCaseService;
    }

    // GET: TestCase/Create
    public IActionResult Create(int testSuiteId)
    {
        var model = new TestCaseDTO { TestSuiteId = testSuiteId };
        return View(model);
    }

    // POST: TestCase/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(TestCaseDTO model)
    {
        if (ModelState.IsValid)
        {
            var testCase = _testCaseService.CreateTestCase(model);
            if (testCase == null)
            {
                ModelState.AddModelError("", "The specified Test Suite does not exist.");
                return View(model);
            }

            return RedirectToAction("Details", "TestSuite", new { id = model.TestSuiteId });
        }

        return View(model);
    }

    // GET: TestCase/Details/5
    public IActionResult Details(int id)
    {
        var testCase = _testCaseService.GetTestCaseById(id);
        if (testCase == null)
        {
            return NotFound();
        }

        return View(testCase);
    }

    // GET: TestCase/Edit/5
    public IActionResult Edit(int id)
    {
        var testCase = _testCaseService.GetTestCaseById(id);
        if (testCase == null)
        {
            return NotFound();
        }

        return View(testCase);
    }

    // POST: TestCase/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, TestCaseDTO model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var updatedTestCase = _testCaseService.UpdateTestCase(model);
            if (updatedTestCase == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", "TestSuite", new { id = updatedTestCase.TestSuiteId });
        }

        return View(model);
    }

    // GET: TestCase/Delete/5
    public IActionResult Delete(int id)
    {
        var testCase = _testCaseService.GetTestCaseById(id);
        if (testCase == null)
        {
            return NotFound();
        }

        return View(testCase);
    }

    // POST: TestCase/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var deletedTestCase = _testCaseService.DeleteTestCase(id);
        if (deletedTestCase == null)
        {
            return NotFound();
        }

        return RedirectToAction("Details", "TestSuite", new { id = deletedTestCase.TestSuiteId });
    }
}
