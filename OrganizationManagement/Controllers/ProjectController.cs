using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.Models;
using OrganizationManagement.DTO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OrganizationManagement.DBContext;

namespace OrganizationManagement.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _tables;

        public ProjectController(ApplicationDbContext tables)
        {
            _tables = tables;
        }

        // GET: Project details
        public IActionResult ProjectDashboard(int projectId)
        {
            var project = _tables.Projects
                .Include(p => p.TestPlans)
                .FirstOrDefault(p => p.ProjectId == projectId);

            if (project == null)
                return NotFound();

            var projectDTO = new ProjectDTO
            {
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                Status = project.Status,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                OrganizationId = project.OrganizationId,
                TestPlans = project.TestPlans.Select(t => new TestPlanDTO
                {
                    TestPlanId = t.TestPlanId,
                    Name = t.Name
                }).ToList()
            };

            return View(projectDTO);
        }

        // GET: Add new project form
        [HttpGet]
        public IActionResult AddProject(int organizationId)
        {
            var dto = new ProjectDTO
            {
                OrganizationId = organizationId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7)
            };
            return View(dto);
        }

        // POST: Add project
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProject(ProjectDTO projectDTO)
        {
            if (ModelState.IsValid)
            {
                var project = new Project
                {
                    ProjectName = projectDTO.ProjectName,
                    Status = projectDTO.Status,
                    Description = projectDTO.Description,
                    StartDate = projectDTO.StartDate,
                    EndDate = projectDTO.EndDate,
                    OrganizationId = projectDTO.OrganizationId
                };

                _tables.Projects.Add(project);
                _tables.SaveChanges();

                return RedirectToAction("OrganizationDashboard", "Organization", new { organizationId = project.OrganizationId });
            }

            return View(projectDTO);
        }

        // POST: Delete project
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProject(int projectId)
        {
            var project = _tables.Projects.FirstOrDefault(p => p.ProjectId == projectId);

            if (project == null)
                return NotFound();

            int organizationId = project.OrganizationId;
            _tables.Projects.Remove(project);
            _tables.SaveChanges();

            return RedirectToAction("OrganizationDashboard", "Organization", new { organizationId = organizationId });
        }
    }
}
