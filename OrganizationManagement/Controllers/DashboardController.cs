using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OrganizationManagement.Models;
using OrganizationManagement.DBContext;
using OrganizationManagement.DTO;

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

        // Index action that loads the page with or without organizations
        public IActionResult Index()
        {
            var orgList = _tables.Organizations
                .Select(o => new OrganizationDTO
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToList();

            // Create the model that will pass the organization list
            var model = new OrganizationDTO
            {
                Organizations = orgList
            };

            if (TempData["Success"] != null)
            {
                ViewBag.SuccessMessage = TempData["Success"].ToString();
            }

            ViewBag.ShowForm = false; // Initially, don't show the form
            return View(model); // Return the model containing the organization list
        }

        // Show the register form when clicked
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ShowRegisterForm()
        {
            ViewBag.ShowForm = true;

            // Fetch organizations list to repopulate after showing form
            var orgList = _tables.Organizations
                .Select(o => new OrganizationDTO
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToList();

            return View("Index", new OrganizationDTO
            {
                Organizations = orgList
            });
        }

        // Register new organization and save to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterName(OrganizationDTO model)
        {
            if (ModelState.IsValid)
            {
                // Create a new organization and add it to the database
                var newOrg = new Organization
                {
                    Name = model.Name
                };

                _tables.Organizations.Add(newOrg);
                _tables.SaveChanges();

                TempData["Success"] = "Organization registered successfully!";
                return RedirectToAction("Index"); // Redirect to Index after successful registration
            }

            // If validation fails, repopulate the organization list
            model.Organizations = _tables.Organizations
                .Select(o => new OrganizationDTO
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToList();

            ViewBag.ShowForm = true; // Show the form again
            return View("Index", model);
        }

        // Action to show organizations list when clicked on "Organizations"
        public IActionResult ViewOrganizations()
        {
            var orgList = _tables.Organizations
                .Select(o => new OrganizationDTO
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToList();

            var model = new OrganizationDTO
            {
                Organizations = orgList
            };

            return PartialView("_OrganizationsList", model); // Return the partial view with the organization list
        }
    }
}
