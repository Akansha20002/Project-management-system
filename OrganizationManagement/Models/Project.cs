using System.ComponentModel.DataAnnotations;

namespace OrganizationManagement.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public string Status { get; set; }

        [Required, MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int OrganizationId { get; set; }

        public ICollection<TestPlan> TestPlans { get; set; }
    }

}
