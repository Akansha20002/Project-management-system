using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.DTO;
using OrganizationManagement.Models;

public class ProjectController : Controller
{
    private readonly IProjectService _service;

    public ProjectController(IProjectService service)
    {
        _service = service;
    }

    public IActionResult ProjectDashboard(int projectId)
    {
        var projectDTO = _service.GetProjectDashboard(projectId);
        if (projectDTO == null)
            return NotFound();

        return View(projectDTO);
    }

    [HttpGet]
    public IActionResult AddProject(int organizationId)
    {
        return View(new ProjectDTO { OrganizationId = organizationId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddProject(ProjectDTO projectDTO)
    {
        if (!ModelState.IsValid)
            return View(projectDTO);

        if (!_service.TryAddProject(projectDTO, out string error))
        {
            ModelState.AddModelError(string.Empty, error);
            return View(projectDTO);
        }

        return RedirectToAction("Dashboard", "Organization", new { organizationId = projectDTO.OrganizationId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteProject(int projectId)
    {
        if (!_service.DeleteProject(projectId, out int organizationId))
            return NotFound();

        return RedirectToAction("Dashboard", "Organization", new { organizationId });
    }

    public IActionResult ProjectDashboardByOrganization(int organizationId)
    {
        var viewModel = _service.GetProjectsByStatus(organizationId);
        return View("ProjectStatusDashboard", viewModel);
    }


//EDIT PROJECT
    [HttpGet]
    public IActionResult EditProject(int projectId)
    {
        var projectDTO = _service.GetProjectDashboard(projectId);
        if (projectDTO == null)
            return NotFound();

        return View(projectDTO); // this returns EditProject.cshtml
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditProject(ProjectDTO dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        if (!_service.TryUpdateProject(dto, out string error))
        {
            ModelState.AddModelError(string.Empty, error);
            return View(dto);
        }

        return RedirectToAction("ProjectDashboard", new { projectId = dto.ProjectId });
    }

}
