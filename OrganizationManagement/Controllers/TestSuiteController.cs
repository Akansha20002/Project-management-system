using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.DBContext;
using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using Microsoft.EntityFrameworkCore;

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

    // GET: Edit Test Suite
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

        
            if (testSuite.TestPlanId == 0)
            {
                
                Console.WriteLine("TestPlanId is missing or incorrect.");
            }

            
            return RedirectToAction("Details", "TestPlan", new { id = testSuite.TestPlanId });
        }

   
        return View(model);
    }

    // GET: Delete Test Suite
    public async Task<IActionResult> Delete(int id)
    {
        var testSuite = await _context.TestSuites
            .Include(ts => ts.TestPlanId)
            .FirstOrDefaultAsync(ts => ts.TestSuiteId == id);

        if (testSuite == null)
        {
            return NotFound();
        }

        return View(testSuite);
    }

    // POST: Delete Test Suite
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
    public IActionResult CreateTestCase(int testSuiteId)
    {
        var model = new TestCaseDTO { TestSuiteId = testSuiteId };
        return View(model);
    }

    // POST: Create Test Case
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTestCase(TestCaseDTO model)
    {
        if (ModelState.IsValid)
        {
            var testCase = new TestCase
            {
                Title = model.Title,
                Description = model.Description,
                Steps = model.Steps,
                IsAutomated = model.IsAutomated,
                TestSuiteId = model.TestSuiteId
            };

            _context.TestCases.Add(testCase);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "TestSuite", new { id = model.TestSuiteId });
        }

        return View(model);
    }
}
