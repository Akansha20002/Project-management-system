using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.Models;
using OrganizationManagement.Services.Interface;
using System.Linq;

namespace OrganizationManagement.Controllers
{
    public class TestPlanController : Controller
    {
        private readonly ITestPlanService _testplanservice;

        public TestPlanController(ITestPlanService testplanservice)
        {
            _testplanservice = testplanservice;
        }

        // GET: Test Plans for the logged-in user and a specific project
        public IActionResult Index(int projectId)
        {
            // Get the userId from the current logged-in user
            var userId = Request.Cookies["UserId"];

            // If there's no userId (user is not logged in), redirect them to the login page or another page
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to view test plans.";
                return RedirectToAction("Login", "Account");  // Assuming you have a login action in the Account controller
            }

            // Fetch test plans based on the current userId
            var tp = _testplanservice.GetTestPlansByUserId(userId);

            // Pass userId and projectId to the view
            ViewBag.UserId = userId;
            ViewBag.ProjectId = projectId;

            return View(tp);  // Return the list of test plans for this user and project
        }

        // GET: Render form to add a test plan
        // GET: Render form to add a test plan
        public IActionResult Add(int projectId)
        {
            var userId = Request.Cookies["UserId"];

            // Check if projectId is 0
            if (string.IsNullOrEmpty(userId) || projectId == 0)
            {
                TempData["ErrorMessage"] = "Invalid Project or User. Please go back and try again.";
                return RedirectToAction("Index", "Dashboard");
            }

            var model = new TestPlan
            {
                CreatedBy = userId,
                ProjectId = projectId  // Set the ProjectId for the model
            };

            return View(model);  // Return the form view with the initialized model
        }

        // POST: Add a new test plan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(TestPlan testPlan)
        {
            // Debugging: Log the ModelState errors if not valid
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);  // Log the errors for debugging
                }

                return View(testPlan);  // Redisplay the form with validation errors
            }

            // If the model is valid, add the test plan to the service
            _testplanservice.Add(testPlan);

            // Redirect to the index page of test plans with the updated projectId
            return RedirectToAction("Index", new { projectId = testPlan.ProjectId });
        }


        public IActionResult Edit(int id)
        {
            var testPlan = _testplanservice.GetTestPlansByUserId(Request.Cookies["UserId"])
                                          .FirstOrDefault(tp => tp.TestPlanId == id);
            if (testPlan == null) return NotFound();

            return View(testPlan);
        }


        // POST REQ FOR UPDATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TestPlan testPlan)
        {
            if (!ModelState.IsValid)
            {
                return View(testPlan);
            }

            _testplanservice.Update(testPlan);
            return RedirectToAction("Index", new { projectId = testPlan.ProjectId });
        }



        //DELETE METHOD POST
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(int id,int projectId)
        {
            var userId = Request.Cookies["UserId"];
            var testPlan = _testplanservice.GetTestPlansByUserId(userId)
                                          .FirstOrDefault(tp => tp.TestPlanId == id);

            if (testPlan == null)
            {
                TempData["ErrorMessage"] = "Test Plan not found.";
                return RedirectToAction("Index", new { projectId });
            }

            _testplanservice.Delete(testPlan);
            return RedirectToAction("Index", new { projectId });

        }
    }
}
