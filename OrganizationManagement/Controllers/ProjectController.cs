using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizationManagement.Models;
using OrganizationManagement.DTO;
using OrganizationManagement.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;

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
                Status = project.Status,
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
            if (ModelState.IsValid)
            {
                // Check for duplicate project name in the same organization
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
                    Status = projectDTO.Status,
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
            if (project == null) return NotFound();

            int organizationId = project.OrganizationId;

            _tables.Projects.Remove(project);
            _tables.SaveChanges();

            return RedirectToAction("Dashboard", "Organization", new { organizationId });
        }
    }
}
