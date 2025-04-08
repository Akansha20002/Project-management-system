using System.ComponentModel.DataAnnotations;

namespace OrganizationManagement.Models
{
    public class TestPlan
    {
        public int TestPlanId { get; set; }

        [Required, MaxLength(10)]
        public string Name { get; set; }

        [Required, MaxLength(500)]
        public string Objective { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public string Strategy { get; set; }

        public int ProjectId { get; set; }


        public ICollection<TestSuite> TestSuites { get; set; }
    }

}
