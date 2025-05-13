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


    public async Task<IActionResult> Details(int id)
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

    public IActionResult Create(int testPlanId)
    {
        var model = new TestSuiteDTO { TestPlanId = testPlanId };
        return View(model);
    }

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

            return RedirectToAction("Details", "TestPlan", new { id = testSuite.TestPlanId });
        }

        return View(model);
    }

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


