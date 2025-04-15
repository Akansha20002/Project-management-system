using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OrganizationManagement.Models;
using OrganizationManagement.DBContext;


namespace OrganizationManagement.Controllers
{
    [Authorize(AuthenticationSchemes = "CustomCookieAuth")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _tables;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ApplicationDbContext tables, ILogger<DashboardController> logger)
        {
            _tables = tables;
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (TempData["Success"] != null)
            {
                ViewBag.SuccessMessage = TempData["Success"].ToString();
            }

            ViewBag.ShowForm = false; 
            return View(new Organization());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ShowRegisterForm()
        {
            ViewBag.ShowForm = true; 
            return View("Index", new Organization());
        }
        public IActionResult RegisterName()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterName(Organization organization)
        {
            if (ModelState.IsValid)
            {
                // Save name to database or logic here
                ViewBag.SuccessMessage = "Organization name registered successfully!";
                return RedirectToAction("Dashboard"); // or wherever you want to go
            }

            return View(organization);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult RegisterOrganization(Organization organization)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _tables.Organizations.Add(organization);
        //        _tables.SaveChanges();

        //        _logger.LogInformation("New organization registered: {Name}", organization.Name);
        //        TempData["Success"] = "Organization registered successfully!";
        //        return RedirectToAction("Index");
        //    }

        //    return View("Index", organization);
        //}
    }
}
