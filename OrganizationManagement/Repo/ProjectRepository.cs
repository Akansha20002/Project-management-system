using Microsoft.EntityFrameworkCore;
using OrganizationManagement.DBContext;
using OrganizationManagement.Models;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Project? GetProjectById(int projectId)
    {
        return _context.Projects
            .Include(p => p.TestPlans)
            .FirstOrDefault(p => p.ProjectId == projectId);
    }

    public List<Project> GetProjectsByOrganizationId(int organizationId)
    {
        return _context.Projects
            .Where(p => p.OrganizationId == organizationId)
            .Include(p => p.TestPlans)
            .ToList();
    }

    public bool ProjectExists(string projectName, int organizationId)
    {
        return _context.Projects.Any(p =>
            p.OrganizationId == organizationId &&
            p.ProjectName.Trim().ToLower() == projectName.Trim().ToLower());
    }

    public void AddProject(Project project)
    {
        _context.Projects.Add(project);
        _context.SaveChanges();
    }

    public void DeleteProject(Project project)
    {
        _context.Projects.Remove(project);
        _context.SaveChanges();
    }

    public void UpdateProjects(List<Project> projects)
    {
        _context.UpdateRange(projects);
        _context.SaveChanges();
    }
}

