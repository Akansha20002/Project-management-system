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

    public IActionResult Create(int testSuiteId)
    {
        var model = new TestCaseDTO
        {
            TestSuiteId = testSuiteId
        };
        return View(model);
    }

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

            await UpdateProjectStatusAsync(model.TestSuiteId);

            return RedirectToAction("Details", "TestSuite", new { id = model.TestSuiteId });
        }

        return View(model);
    }

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

            model.Steps = FormatSteps(model.Steps);

            testCase.Title = model.Title;
            testCase.Description = model.Description;
            testCase.Steps = model.Steps;
            testCase.IsAutomated = model.IsAutomated;

            _context.Update(testCase);
            await _context.SaveChangesAsync();

            await UpdateProjectStatusAsync(testCase.TestSuiteId);

            return RedirectToAction("Details", "TestSuite", new { id = testCase.TestSuiteId });
        }

        return View(model);
    }

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

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var testCase = await _context.TestCases.FindAsync(id);
        if (testCase != null)
        {
            int suiteId = testCase.TestSuiteId;
            _context.TestCases.Remove(testCase);
            await _context.SaveChangesAsync();

            await UpdateProjectStatusAsync(suiteId);
        }

        return RedirectToAction("Details", "TestSuite", new { id = testCase.TestSuiteId });
    }

    private string FormatSteps(string steps)
    {
        if (string.IsNullOrWhiteSpace(steps))
            return steps;

        var lines = steps
            .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        if (lines.All(line => line.Trim().StartsWith("Step ")))
        {
            return steps;
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

    // ✅ UPDATED: Ignore test steps — only check for test plans, suites, and cases
    private async Task UpdateProjectStatusAsync(int testSuiteId)
    {
        var testSuite = await _context.TestSuites
            .Include(ts => ts.TestPlan)
                .ThenInclude(tp => tp.Project)
            .Include(ts => ts.TestCases)
            .FirstOrDefaultAsync(ts => ts.TestSuiteId == testSuiteId);

        if (testSuite?.TestPlan?.Project == null) return;

        var project = testSuite.TestPlan.Project;

        var allTestPlans = await _context.TestsPlans
            .Where(tp => tp.ProjectId == project.ProjectId)
            .Include(tp => tp.TestSuites)
                .ThenInclude(ts => ts.TestCases)
            .ToListAsync();

        bool allCompleted = allTestPlans.All(tp =>
            tp.TestSuites != null && tp.TestSuites.Any() &&
            tp.TestSuites.All(ts =>
                ts.TestCases != null && ts.TestCases.Any()
            )
        );

        if (allCompleted && project.Status != "Completed")
        {
            project.Status = "Completed";
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }
        else if (!allCompleted && project.Status == "Completed")
        {
            project.Status = "In Progress"; // Optional fallback
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }
    }
}
