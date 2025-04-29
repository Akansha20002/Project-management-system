using OrganizationManagement.DTO;

namespace OrganizationManagement.ViewModels
{
    public class ProjectStatusGroupViewModel
    {
        public List<ProjectDTO> CompletedProjects { get; set; }
        public List<ProjectDTO> IncompleteProjects { get; set; }
        public List<ProjectDTO> PendingProjects { get; set; }
    }
}
