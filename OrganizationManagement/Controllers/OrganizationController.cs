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

            if (!Request.Cookies.TryGetValue("UserId", out string userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

         
            var organization = await _tables.Organizations
                .Include(o => o.Projects) 
                .FirstOrDefaultAsync(o => o.Id == organizationId && o.CreatedBy == userId);

            if (organization == null)
            {
                return Unauthorized(); 
            }


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

           
            ViewBag.OrganizationId = organizationId;
            ViewBag.UserName = Request.Cookies["UserName"];
            return View(projectDTOs); 
        }

     
    }
}
