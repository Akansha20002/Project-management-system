using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using OrganizationManagement.ViewModels;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repo;

    public ProjectService(IProjectRepository repo)
    {
        _repo = repo;
    }

    public ProjectDTO? GetProjectDashboard(int projectId)
    {
        var project = _repo.GetProjectById(projectId);
        if (project == null) return null;

        return new ProjectDTO
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
    }

    public bool TryAddProject(ProjectDTO dto, out string error)
    {
        error = string.Empty;

        if (dto.StartDate.Date < DateTime.UtcNow.Date)
            error = "Start date cannot be in the past.";
        else if (dto.EndDate.Date < DateTime.UtcNow.Date)
            error = "End date cannot be in the past.";
        else if (dto.EndDate.Date < dto.StartDate.Date)
            error = "End date cannot be before start date.";
        else if (_repo.ProjectExists(dto.ProjectName, dto.OrganizationId))
            error = "A project with the same name already exists in this organization.";

        if (!string.IsNullOrEmpty(error)) return false;

        var project = new Project
        {
            ProjectName = dto.ProjectName,
            Status = dto.Status,
            Description = dto.Description,
            StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc),
            EndDate = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Utc),
            OrganizationId = dto.OrganizationId,
            TestPlans = new List<TestPlan>()
        };

        _repo.AddProject(project);
        return true;
    }

    public bool DeleteProject(int projectId, out int organizationId)
    {
        var project = _repo.GetProjectById(projectId);
        organizationId = 0;
        if (project == null) return false;

        organizationId = project.OrganizationId;
        _repo.DeleteProject(project);
        return true;
    }

    public ProjectStatusGroupViewModel GetProjectsByStatus(int organizationId)
    {
        var projects = _repo.GetProjectsByOrganizationId(organizationId);
        bool changesMade = false;

        foreach (var p in projects)
        {
            if (p.Status.ToLower() != "completed" && p.EndDate < DateTime.UtcNow)
            {
                if (p.Status.ToLower() != "incomplete")
                {
                    p.Status = "Incomplete";
                    changesMade = true;
                }
            }
        }

        if (changesMade)
            _repo.UpdateProjects(projects);

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
                case "completed": completed.Add(dto); break;
                case "incomplete": incomplete.Add(dto); break;
                default: pending.Add(dto); break;
            }
        }

        return new ProjectStatusGroupViewModel
        {
            CompletedProjects = completed,
            IncompleteProjects = incomplete,
            PendingProjects = pending
        };
    }
}

