using System;
using System.ComponentModel.DataAnnotations;

namespace OrganizationManagement.DTO
{
    public class ProjectDTO
    {
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage = "Project name can't exceed 200 characters.")]
        public string ProjectName { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        [MaxLength(500, ErrorMessage = "Description can't exceed 500 characters.")]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int OrganizationId { get; set; }

      
        public ICollection<TestPlanDTO> TestPlans { get; set; }
    }
}
