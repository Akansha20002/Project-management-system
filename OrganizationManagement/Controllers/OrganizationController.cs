using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using OrganizationManagement.Services.Interface;

namespace OrganizationManagement.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        public IActionResult Dashboard(int organizationId)
        {
            if (!Request.Cookies.TryGetValue("UserId", out string userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var organizations = _organizationService.GetOrganizationsByUserId(userId);
            var organization = organizations.FirstOrDefault(o => o.Id == organizationId);

            if (organization == null)
            {
                return Unauthorized();
            }

            var projectDTOs = (organization.Projects ?? new List<Project>())
       .Select(p => new ProjectDTO
       {
           ProjectId = p.ProjectId,
           ProjectName = p.ProjectName,
           Status = p.Status,
           Description = p.Description,
           StartDate = p.StartDate.ToUniversalTime(),
           EndDate = p.EndDate.ToUniversalTime(),
           OrganizationId = p.OrganizationId
       }).ToList();


            ViewBag.OrganizationId = organizationId;
            ViewBag.UserName = Request.Cookies["UserName"];
            return View(projectDTOs);
        }
    }
}
