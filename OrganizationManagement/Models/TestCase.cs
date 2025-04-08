using System.ComponentModel.DataAnnotations;

namespace OrganizationManagement.Models
{
    public class TestCase
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required, MaxLength(200)]
        public string Description { get; set; }

        [Required]
        public string Steps { get; set; }

        public bool IsAutomated { get; set; } = false;

        public int TestSuiteId { get; set; }

        public ICollection<TestStep> TestSteps { get; set; }
    }

}
