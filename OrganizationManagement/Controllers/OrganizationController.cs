using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizationManagement.Models;
using OrganizationManagement.DTO;
using System.Linq;
using OrganizationManagement.DBContext;
using System.Threading.Tasks;

namespace OrganizationManagement.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly ApplicationDbContext _tables;

        public OrganizationController(ApplicationDbContext tables)
        {
            _tables = tables;
        }

        public async Task<IActionResult> Dashboard(int organizationId)
        {
            // Get the current user ID from cookies
            if (!Request.Cookies.TryGetValue("UserId", out string userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch organization with projects and check if it's owned by this user
            var organization = await _tables.Organizations
                .Include(o => o.Projects) // Load projects
                .FirstOrDefaultAsync(o => o.Id == organizationId && o.CreatedBy == userId);

            if (organization == null)
            {
                return Unauthorized(); // Prevent access if not their organization
            }

            // Map the Projects to ProjectDTO
            var projectDTOs = organization.Projects.Select(p => new ProjectDTO
            {
                ProjectId = p.ProjectId,
                ProjectName = p.ProjectName,
                Status = p.Status,
                Description = p.Description,
                StartDate = p.StartDate.ToUniversalTime(),
                EndDate = p.EndDate.ToUniversalTime(),
                OrganizationId = p.OrganizationId,
                // TestPlans = p.TestPlans.Select(t => new TestPlanDTO
                // {
                //     TestPlanId = t.TestPlanId,
                //     Name = t.Name
                // }).ToList()
            }).ToList();

            // Pass the data to the view
            ViewBag.OrganizationId = organizationId;
            ViewBag.UserName = Request.Cookies["UserName"];
            return View(projectDTOs); // Pass DTOs instead of Entity
        }

        // Add more actions like Create, Delete, etc.
    }
}
