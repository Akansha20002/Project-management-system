using System.ComponentModel.DataAnnotations;

namespace OrganizationManagement.Models
{
    public class TestSuite
    {
        public int TestSuiteId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, MaxLength(500)]
        public string Description { get; set; }

        public int TestPlanId { get; set; }


        public ICollection<TestCase> TestCases { get; set; }
    }

}
