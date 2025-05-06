using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizationManagement.Models;
using OrganizationManagement.DTO;
using OrganizationManagement.DBContext;
using System.Linq;
using OrganizationManagement.ViewModels;

namespace OrganizationManagement.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _tables;

        public ProjectController(ApplicationDbContext tables)
        {
            _tables = tables;
        }

     
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
                Status = project.Status.ToString(),
                Description = project.Description,
                StartDate = DateTime.SpecifyKind(project.StartDate, DateTimeKind.Utc),
                EndDate = DateTime.SpecifyKind(project.EndDate, DateTimeKind.Utc),
                OrganizationId = project.OrganizationId,
                TestPlans = project.TestPlans?.Select(t => new TestPlanDTO
                {
                    TestPlanId = t.TestPlanId,
                    Name = t.Name
                }).ToList()
            };

            return View(projectDTO);
        }

   
        [HttpGet]
        public IActionResult AddProject(int organizationId)
        {
            var dto = new ProjectDTO { OrganizationId = organizationId };
            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProject(ProjectDTO projectDTO)
        {
            if (projectDTO.StartDate.Date < DateTime.UtcNow.Date)
            {
                ModelState.AddModelError("StartDate", "Start date cannot be in the past.");
            }

            if (projectDTO.EndDate.Date < DateTime.UtcNow.Date)
            {
                ModelState.AddModelError("EndDate", "End date cannot be in the past.");
            }

            if (projectDTO.EndDate.Date < projectDTO.StartDate.Date)
            {
                ModelState.AddModelError("EndDate", "End date cannot be before start date.");
            }

            if (ModelState.IsValid)
            {
                bool projectExists = _tables.Projects.Any(p =>
                    p.OrganizationId == projectDTO.OrganizationId &&
                    p.ProjectName.Trim().ToLower() == projectDTO.ProjectName.Trim().ToLower());

                if (projectExists)
                {
                    ModelState.AddModelError("ProjectName", "A project with the same name already exists in this organization.");
                    return View(projectDTO);
                }

                var project = new Project
                {
                    ProjectName = projectDTO.ProjectName,
                    Status = projectDTO.Status.ToString(),
                    Description = projectDTO.Description,
                    StartDate = DateTime.SpecifyKind(projectDTO.StartDate, DateTimeKind.Utc),
                    EndDate = DateTime.SpecifyKind(projectDTO.EndDate, DateTimeKind.Utc),
                    OrganizationId = projectDTO.OrganizationId,
                    TestPlans = new List<TestPlan>() // Initialize empty list
                };

                _tables.Projects.Add(project);
                _tables.SaveChanges();

                return RedirectToAction("Dashboard", "Organization", new { organizationId = project.OrganizationId });
            }

            return View(projectDTO);
        }

      
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

            return RedirectToAction("Dashboard", "Organization", new { organizationId });
        }

        public IActionResult ProjectDashboardByOrganization(int organizationId)
        {
            var projects = _tables.Projects
                .Where(p => p.OrganizationId == organizationId)
                .Include(p => p.TestPlans)
                .ToList();

            bool changesMade = false;

            foreach (var project in projects)
            {
              
                if (project.Status.ToLower() != "completed" && project.EndDate < DateTime.UtcNow)
                {
                    if (project.Status.ToLower() != "incomplete")
                    {
                        project.Status = "Incomplete";
                        changesMade = true;
                    }
                }
            }

            if (changesMade)
            {
                _tables.SaveChanges();
            }

            var completed = new List<ProjectDTO>();
            var incomplete = new List<ProjectDTO>();
            var pending = new List<ProjectDTO>();

            foreach (var p in projects)
            {
                var dto = new ProjectDTO
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    Status = p.Status,
                    Description = p.Description,
                    StartDate = DateTime.SpecifyKind(p.StartDate, DateTimeKind.Utc),
                    EndDate = DateTime.SpecifyKind(p.EndDate, DateTimeKind.Utc),
                    OrganizationId = p.OrganizationId
                };

                switch (p.Status.ToLower())
                {
                    case "completed":
                        completed.Add(dto);
                        break;
                    case "incomplete":
                        incomplete.Add(dto);
                        break;
                    default:
                        pending.Add(dto);
                        break;
                }
            }

            var viewModel = new ProjectStatusGroupViewModel
            {
                CompletedProjects = completed,
                IncompleteProjects = incomplete,
                PendingProjects = pending
            };

            return View("ProjectStatusDashboard", viewModel);
        }
    }
}
