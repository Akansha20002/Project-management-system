using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizationManagement.DBContext;
using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

public class TestCaseController : Controller
{
    private readonly ApplicationDbContext _context;

    public TestCaseController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: TestCase/Create
    public IActionResult Create(int testSuiteId)
    {
        var model = new TestCaseDTO
        {
            TestSuiteId = testSuiteId
        };
        return View(model);
    }

    // POST: TestCase/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TestCaseDTO model)
    {
        if (ModelState.IsValid)
        {
            var testSuiteExists = await _context.TestSuites.AnyAsync(ts => ts.TestSuiteId == model.TestSuiteId);
            if (!testSuiteExists)
            {
                ModelState.AddModelError("", "The specified Test Suite does not exist.");
                return View(model);
            }

            // ✅ Format steps with numbering
            model.Steps = FormatSteps(model.Steps);

            var testCase = new TestCase
            {
                Title = model.Title,
                Description = model.Description,
                Steps = model.Steps,
                TestSuiteId = model.TestSuiteId,
                IsAutomated = model.IsAutomated
            };

            _context.Add(testCase);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "TestSuite", new { id = model.TestSuiteId });
        }

        return View(model);
    }

    // GET: TestCase/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var testCase = await _context.TestCases.FirstOrDefaultAsync(tc => tc.Id == id);

        if (testCase == null)
        {
            return NotFound();
        }

        var model = new TestCaseDTO
        {
            Id = testCase.Id,
            Title = testCase.Title,
            Description = testCase.Description,
            Steps = testCase.Steps,
            TestSuiteId = testCase.TestSuiteId,
            IsAutomated = testCase.IsAutomated
        };

        return View(model);
    }

    // GET: TestCase/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var testCase = await _context.TestCases.FindAsync(id);
        if (testCase == null)
        {
            return NotFound();
        }

        var model = new TestCaseDTO
        {
            Id = testCase.Id,
            Title = testCase.Title,
            Description = testCase.Description,
            Steps = testCase.Steps,
            TestSuiteId = testCase.TestSuiteId,
            IsAutomated = testCase.IsAutomated
        };

        return View(model);
    }

    // POST: TestCase/Edit/5
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Edit(int id, TestCaseDTO model)
    //{
    //    if (id != model.Id)
    //    {
    //        return NotFound();
    //    }

    //    if (ModelState.IsValid)
    //    {
    //        try
    //        {
    //            var testCase = await _context.TestCases.FindAsync(id);
    //            if (testCase == null)
    //            {
    //                return NotFound();
    //            }

    //            // ✅ Format steps with numbering only if not already formatted
    //            model.Steps = FormatSteps(model.Steps);

    //            testCase.Title = model.Title;
    //            testCase.Description = model.Description;
    //            testCase.Steps = model.Steps;
    //            testCase.IsAutomated = model.IsAutomated;

    //            _context.Update(testCase);
    //            await _context.SaveChangesAsync();

    //            return RedirectToAction("Details", "TestSuite", new { id = testCase.TestSuiteId });
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!TestCaseExists(model.Id))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }
    //    }

    //    return View(model);
    //}
    // POST: TestCase/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TestCaseDTO model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var testCase = await _context.TestCases.FindAsync(id);
            if (testCase == null)
            {
                return NotFound();
            }

            // ✅ Format steps with numbering only if not already formatted
            model.Steps = FormatSteps(model.Steps);

            testCase.Title = model.Title;
            testCase.Description = model.Description;
            testCase.Steps = model.Steps;
            testCase.IsAutomated = model.IsAutomated;

            _context.Update(testCase);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "TestSuite", new { id = testCase.TestSuiteId });
        }

        return View(model);
    }


    // GET: TestCase/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var testCase = await _context.TestCases.FirstOrDefaultAsync(tc => tc.Id == id);
        if (testCase == null)
        {
            return NotFound();
        }

        var model = new TestCaseDTO
        {
            Id = testCase.Id,
            Title = testCase.Title,
            Description = testCase.Description,
            Steps = testCase.Steps,
            TestSuiteId = testCase.TestSuiteId,
            IsAutomated = testCase.IsAutomated
        };

        return View(model);
    }

    // POST: TestCase/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var testCase = await _context.TestCases.FindAsync(id);
        if (testCase != null)
        {
            _context.TestCases.Remove(testCase);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Details", "TestSuite", new { id = testCase.TestSuiteId });
    }

    // ✅ Helper method to format steps only if not already numbered
    private string FormatSteps(string steps)
    {
        if (string.IsNullOrWhiteSpace(steps))
            return steps;

        var lines = steps
            .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        // Check if all lines are already numbered with "Step X:"
        if (lines.All(line => line.Trim().StartsWith("Step ")))
        {
            return steps; // Already formatted
        }

        var formatted = lines
            .Select((line, index) => $"Step {index + 1}: {line.Trim()}")
            .ToArray();

        return string.Join(Environment.NewLine, formatted);
    }

    private bool TestCaseExists(int id)
    {
        return _context.TestCases.Any(e => e.Id == id);
    }
}
