using System;
using System.ComponentModel.DataAnnotations;

namespace OrganizationManagement.DTO
{
    public class ProjectDTO
    {
        public int ProjectId { get; set; }

        
        [MaxLength(200, ErrorMessage = "Project name can't exceed 200 characters.")]
        public string ProjectName { get; set; }

  
        public string Status { get; set; }

       
        [MaxLength(500, ErrorMessage = "Description can't exceed 500 characters.")]
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        
        public DateTime EndDate { get; set; }

        
        public int OrganizationId { get; set; }


        public List<TestPlanDTO>? TestPlans { get; set; }

        public bool IsCompleted => Status?.ToLower() == "completed";
        public bool IsIncomplete => Status?.ToLower() == "incomplete";
        public bool IsPastDeadline => !IsCompleted && EndDate < DateTime.UtcNow;
        public List<ProjectDTO> CompletedProjects { get; set; } = new();
        public List<ProjectDTO> IncompleteProjects { get; set; } = new();
        public List<ProjectDTO> PendingProjects { get; set; } = new();

    }
}
