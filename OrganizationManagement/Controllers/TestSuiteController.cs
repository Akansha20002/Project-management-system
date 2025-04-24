using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace OrganizationManagement.Controllers
{
    public class TestSuiteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestSuiteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TestSuite/Create
        public IActionResult Create(int testPlanId)
        {
            ViewBag.TestPlanId = testPlanId;
            return View();
        }

        // POST: TestSuite/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TestSuiteDTO testSuiteDto)
        {
            if (ModelState.IsValid)
            {
                // Map the DTO to the entity (TestSuite)
                var testSuite = new TestSuite
                {
                    Name = testSuiteDto.Name,
                    Description = testSuiteDto.Description,
                    TestPlanId = testSuiteDto.TestPlanId
                };

                // Add the test suite to the database
                _context.Add(testSuite);
                await _context.SaveChangesAsync();

                // Redirect to the index page of the test plan (or wherever you want to redirect)
                return RedirectToAction("Index", "TestPlan", new { id = testSuite.TestPlanId });
            }

            // If validation fails, return the view with the same data
            return View(testSuiteDto);
        }
    }
}
