using System.ComponentModel.DataAnnotations;

namespace OrganizationManagement.DTO
{
    public class TestPlanDTO
    {
            public int TestPlanId { get; set; }

            [Required]
            [MaxLength(10, ErrorMessage = "Test Plan name can't exceed 10 characters.")]
            public string Name { get; set; }

            [Required]
            [MaxLength(500, ErrorMessage = "Objective can't exceed 500 characters.")]
            public string Objective { get; set; }

            [Required]
            public string CreatedBy { get; set; }

            [Required]
            public string Strategy { get; set; }

            [Required]
            public int ProjectId { get; set; }

            // Optionally, you can include TestSuites here
            public ICollection<TestSuiteDTO> TestSuites { get; set; }
        }

    
}


