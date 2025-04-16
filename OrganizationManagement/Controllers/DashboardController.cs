using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.DBContext;
using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using System.Security.Claims;

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

            if (TempData["Success"] != null)
                ViewBag.SuccessMessage = TempData["Success"].ToString();

            if (TempData["Error"] != null)
                ViewBag.ErrorMessage = TempData["Error"].ToString();

            ViewBag.ShowForm = false;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ShowRegisterForm()
        {
            ViewBag.ShowForm = true;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterName(OrganizationDTO model)
        {
            if (_tables.Organizations.Any(o => o.Name == model.Name))
            {
                TempData["Error"] = "An organization with this name already exists.";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var newOrg = new Organization
                {
                    Name = model.Name,
                    CreatedBy= currentUserId
                  
                };

                _tables.Organizations.Add(newOrg);
                _tables.SaveChanges();

                TempData["Success"] = "Organization registered successfully!";
                return RedirectToAction("Index");
            }

            model.Organizations = _tables.Organizations
                .Select(o => new OrganizationDTO
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToList();

            ViewBag.ShowForm = true;
            return View("Index", model);
        }
    }
}
