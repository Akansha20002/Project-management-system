using OrganizationManagement.DTO;
using OrganizationManagement.ViewModels;

public interface IProjectService
{
    ProjectDTO? GetProjectDashboard(int projectId);
    bool TryAddProject(ProjectDTO dto, out string error);
    bool DeleteProject(int projectId, out int organizationId);
    ProjectStatusGroupViewModel GetProjectsByStatus(int organizationId);
}
