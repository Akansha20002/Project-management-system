using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizationManagement.DBContext;
using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using System.Linq;
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
                    CreatedBy = currentUserId
                };

                _tables.Organizations.Add(newOrg);
                _tables.SaveChanges();

                TempData["Success"] = "Organization registered successfully!";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var organization = _tables.Organizations
                .Include(o => o.Projects)
                .FirstOrDefault(o => o.Id == id);

            if (organization == null)
            {
                TempData["Error"] = "Organization not found.";
                return RedirectToAction("Index");
            }

            var model = new OrganizationDetailsDTO
            {
                Id = organization.Id,
                Name = organization.Name,
                Projects = organization.Projects.Select(p => new ProjectDTO
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    Status = p.Status
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var organization = _tables.Organizations
                .Include(o => o.Projects)
                .FirstOrDefault(o => o.Id == id);

            if (organization == null)
            {
                TempData["Error"] = "Organization not found.";
                return RedirectToAction("Index");
            }

            try
            {
                if (organization.Projects != null && organization.Projects.Any())
                {
                    _tables.Projects.RemoveRange(organization.Projects);
                }

                _tables.Organizations.Remove(organization);
                _tables.SaveChanges();

                TempData["Success"] = "Organization and its projects deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting organization: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}
