using System.Collections.Generic;

namespace OrganizationManagement.DTO
{
    public class ProjectStatusGroupViewModel
    {
        public List<ProjectDTO> CompletedProjects { get; set; } = new();
        public List<ProjectDTO> IncompleteProjects { get; set; } = new();
        public List<ProjectDTO> PendingProjects { get; set; } = new();
    }
}
