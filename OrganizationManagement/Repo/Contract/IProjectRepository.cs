using OrganizationManagement.Models;

public interface IProjectRepository
{
    Project? GetProjectById(int projectId);
    List<Project> GetProjectsByOrganizationId(int organizationId);
    bool ProjectExists(string projectName, int organizationId);
    void AddProject(Project project);
    void DeleteProject(Project project);
    void UpdateProjects(List<Project> projects);
}
