using OrganizationManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace OrganizationManagement.DTO
{
    public class TestSuiteDTO
    {
        public int TestSuiteId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public int TestPlanId { get; set; }


        public ICollection<TestCase>? TestCases { get; set; }
    }
}
