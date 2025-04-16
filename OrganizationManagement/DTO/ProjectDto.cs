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

      
        public ICollection<TestPlanDTO> TestPlans { get; set; }
    }
}
