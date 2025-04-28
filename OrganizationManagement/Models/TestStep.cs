using System.ComponentModel.DataAnnotations;

namespace OrganizationManagement.Models
{
    public class TestStep
    {
        public int Id { get; set; }

        public int StepNumber { get; set; }

        [Required]
        public string Action { get; set; }

        [Required]
        public string ExpectedResult { get; set; }

        [Required]
        public string ActualResult { get; set; }

        public int TestCaseId { get; set; }
    }
}
