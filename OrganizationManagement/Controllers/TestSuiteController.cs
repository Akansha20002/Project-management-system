using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizationManagement.DBContext;
using OrganizationManagement.Models;
using OrganizationManagement.DTO;
using System.Threading.Tasks;

public class TestSuiteController : Controller
{
    private readonly ApplicationDbContext _context;

    public TestSuiteController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: TestSuite/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var testSuite = await _context.TestSuites
            .Include(ts => ts.TestCases) // Include related TestCases
            .FirstOrDefaultAsync(ts => ts.TestSuiteId == id);

        if (testSuite == null)
        {
            return NotFound();
        }

        return View(testSuite); // Return TestSuite details with associated TestCases
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

    // GET: TestSuite/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var testSuite = await _context.TestSuites.FindAsync(id);
        if (testSuite == null)
        {
            return NotFound();
        }

        var model = new TestSuiteDTO
        {
            TestSuiteId = testSuite.TestSuiteId,
            Name = testSuite.Name,
            Description = testSuite.Description,
            TestPlanId = testSuite.TestPlanId
        };

        return View(model);
    }

    // POST: TestSuite/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TestSuiteDTO model)
    {
        if (id != model.TestSuiteId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var testSuite = await _context.TestSuites.FindAsync(id);
            if (testSuite == null)
            {
                return NotFound();
            }

            testSuite.Name = model.Name;
            testSuite.Description = model.Description;

            _context.Update(testSuite);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "TestPlan", new { id = testSuite.TestPlanId });
        }

        return View(model);
    }

    // GET: TestSuite/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var testSuite = await _context.TestSuites
            .Include(ts => ts.TestCases)
            .FirstOrDefaultAsync(ts => ts.TestSuiteId == id);

        if (testSuite == null)
        {
            return NotFound();
        }

        return View(testSuite);
    }

    // POST: TestSuite/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var testSuite = await _context.TestSuites.FindAsync(id);
        if (testSuite != null)
        {
            _context.TestSuites.Remove(testSuite);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Details", "TestPlan", new { id = testSuite.TestPlanId });
    }
}


