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
            var testCase = new TestCase
            {
                Title = model.Title,
                Description = model.Description,
                TestSuiteId = model.TestSuiteId,
                IsAutomated = model.IsAutomated
            };

            _context.TestCases.Add(testCase);
            await _context.SaveChangesAsync();

            foreach (var step in model.TestSteps)
            {
                var testStep = new TestStep
                {
                    TestCaseId = testCase.Id,
                    StepNumber = step.StepNumber,
                    ExpectedResult = step.ExpectedResult,
                    ActualResult = step.ActualResult
                };
                _context.TestSteps.Add(testStep);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = testCase.Id });
        }

        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var testCase = await _context.TestCases
            .Include(tc => tc.TestSteps)
            .FirstOrDefaultAsync(tc => tc.Id == id);

        if (testCase == null) return NotFound();

        var model = new TestCaseDTO
        {
            Id = testCase.Id,
            Title = testCase.Title,
            Description = testCase.Description,
            IsAutomated = testCase.IsAutomated,
            TestSteps = testCase.TestSteps.Select(ts => new TestStepDTO
            {
                Id = ts.Id,
                StepNumber = ts.StepNumber,
                ExpectedResult = ts.ExpectedResult,
                ActualResult = ts.ActualResult
            }).ToList()
        };

        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var testCase = await _context.TestCases
            .Include(tc => tc.TestSteps)
            .FirstOrDefaultAsync(tc => tc.Id == id);

        if (testCase == null) return NotFound();

        var model = new TestCaseDTO
        {
            Id = testCase.Id,
            Title = testCase.Title,
            Description = testCase.Description,
            IsAutomated = testCase.IsAutomated,
            TestSteps = testCase.TestSteps.Select(ts => new TestStepDTO
            {
                Id = ts.Id,
                StepNumber = ts.StepNumber,
                ExpectedResult = ts.ExpectedResult,
                ActualResult = ts.ActualResult
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TestCaseDTO model)
    {
        if (ModelState.IsValid)
        {
            var testCase = await _context.TestCases
                .Include(tc => tc.TestSteps)
                .FirstOrDefaultAsync(tc => tc.Id == id);

            if (testCase == null) return NotFound();

            testCase.Title = model.Title;
            testCase.Description = model.Description;
            testCase.IsAutomated = model.IsAutomated;

            foreach (var stepDTO in model.TestSteps)
            {
                var existingStep = testCase.TestSteps.FirstOrDefault(ts => ts.Id == stepDTO.Id);

                if (existingStep != null)
                {
                    existingStep.ExpectedResult = stepDTO.ExpectedResult;
                    existingStep.ActualResult = stepDTO.ActualResult;
                }
                else
                {
                    var newStep = new TestStep
                    {
                        TestCaseId = testCase.Id,
                        StepNumber = stepDTO.StepNumber,
                        ExpectedResult = stepDTO.ExpectedResult,
                        ActualResult = stepDTO.ActualResult
                    };
                    _context.TestSteps.Add(newStep);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = testCase.Id });
        }

        return View(model);
    }
}
