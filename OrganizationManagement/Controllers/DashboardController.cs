using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using OrganizationManagement.Services.Interface;
using System.Security.Claims;
using System.Linq;

namespace OrganizationManagement.Controllers
{
    [Authorize(AuthenticationSchemes = "CustomCookieAuth")]
    public class DashboardController : Controller
    {
        private readonly IOrganizationService _organizationService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IOrganizationService organizationService, ILogger<DashboardController> logger)
        {
            _organizationService = organizationService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var orgList = _organizationService.GetOrganizations()
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

            var orgList = _organizationService.GetOrganizations()
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
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var exists = _organizationService.OrganizationExists(currentUserId, model.Name);
            if (exists)
            {
                TempData["Error"] = "An organization with this name already exists.";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var newOrg = new Organization
                {
                    Name = model.Name,
                    CreatedBy = currentUserId
                };

                _organizationService.Add(newOrg);

                TempData["Success"] = "Organization registered successfully!";
                return RedirectToAction("Index");
            }

            model.Organizations = _organizationService.GetOrganizations()
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
