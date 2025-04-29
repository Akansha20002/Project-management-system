using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.DTO;

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
    public IActionResult AddProject(ProjectDTO dto)
    {
        if (!_service.TryAddProject(dto, out string error))
        {
            if (!string.IsNullOrEmpty(error))
                ModelState.AddModelError(string.Empty, error);

            return View(dto);
        }

        return RedirectToAction("Dashboard", "Organization", new { organizationId = dto.OrganizationId });
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
}

