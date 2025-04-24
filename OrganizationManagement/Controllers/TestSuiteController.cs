using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.DBContext;
using OrganizationManagement.DTO;
using OrganizationManagement.Models;

public class TestSuiteController : Controller
{
    private readonly ApplicationDbContext _context;

    public TestSuiteController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Create Test Suite
    public IActionResult Create(int testPlanId)
    {
        var model = new TestSuiteDTO { TestPlanId = testPlanId };
        return View(model);
    }

    // POST: Create Test Suite
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TestSuiteDTO model)
    {
        if (ModelState.IsValid)
        {
            var testSuite = new TestSuite
            {
                Name = model.Name,
                Description = model.Description,
                TestPlanId = model.TestPlanId
            };

            _context.TestSuites.Add(testSuite);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "TestPlan", new { id = model.TestPlanId });
        }

        return View(model);
    }
}
