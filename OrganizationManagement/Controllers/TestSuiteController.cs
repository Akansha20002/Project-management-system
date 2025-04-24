using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.Models;
using OrganizationManagement.Services.Interface;
using System.Linq;

namespace OrganizationManagement.Controllers
{
    public class TestSuiteController : Controller
    {
        private readonly ITestSuitesService _testSuitesService;

        public TestSuiteController(ITestSuitesService testSuitesService)
        {
            _testSuitesService = testSuitesService;
        }

        // GET: List of test suites for a user and test plan
        public IActionResult Index(int testPlanId)
        {
            var userIdCookie = Request.Cookies["UserId"];

            if (string.IsNullOrEmpty(userIdCookie) || !int.TryParse(userIdCookie, out int userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to view test suites.";
                return RedirectToAction("Login", "Account");
            }

            var suites = _testSuitesService.GetTestSuiteByUserId(userId)
                                           .Where(ts => ts.TestPlanId == testPlanId)
                                           .ToList();

            ViewBag.UserId = userId;
            ViewBag.TestPlanId = testPlanId;

            return View(suites);
        }

        // GET: Render form to add a test suite
        public IActionResult Add(int testPlanId)
        {
            if (testPlanId == 0)
            {
                TempData["ErrorMessage"] = "Invalid Test Plan.";
                return RedirectToAction("Index", "Dashboard");
            }

            var model = new TestSuite
            {
                TestPlanId = testPlanId
            };

            return View(model);
        }

        // POST: Add a new test suite
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(TestSuite testSuite)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                return View(testSuite);
            }

            _testSuitesService.Add(testSuite);
            return RedirectToAction("Index", new { testPlanId = testSuite.TestPlanId });
        }

        // GET: Edit form
        public IActionResult Edit(int TestSuiteId)
        {
            var userIdCookie = Request.Cookies["UserId"];

            if (string.IsNullOrEmpty(userIdCookie) || !int.TryParse(userIdCookie, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var suite = _testSuitesService.GetTestSuiteByUserId(userId)
                                          .FirstOrDefault(ts => ts.TestSuiteId == TestSuiteId);

            if (suite == null)
                return NotFound();

            return View(suite);
        }

        // POST: Edit update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TestSuite testSuite)
        {
            if (!ModelState.IsValid)
            {
                return View(testSuite);
            }

            _testSuitesService.Update(testSuite);
            return RedirectToAction("Index", new { testPlanId = testSuite.TestPlanId });
        }

        // POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, int testPlanId)
        {
            var userIdCookie = Request.Cookies["UserId"];

            if (string.IsNullOrEmpty(userIdCookie) || !int.TryParse(userIdCookie, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var suite = _testSuitesService.GetTestSuiteByUserId(userId)
                                          .FirstOrDefault(ts => ts.TestSuiteId == id);

            if (suite == null)
            {
                TempData["ErrorMessage"] = "Test Suite not found.";
                return RedirectToAction("Index", new { testPlanId });
            }

            _testSuitesService.Delete(suite);
            return RedirectToAction("Index", new { testPlanId });
        }
    }
}
