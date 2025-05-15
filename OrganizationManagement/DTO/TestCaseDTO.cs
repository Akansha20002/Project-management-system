using System.ComponentModel.DataAnnotations;
using OrganizationManagement.Models;

namespace OrganizationManagement.DTO
{
    public class TestCaseDTO
    {
        public int Id { get; set; }

        public int TestSuiteId { get; set; }
       
    [Required]
    public string Title { get; set; }

    [Required, MaxLength(200)]
    public string Description { get; set; }

    
    //public string Steps { get; set; }

    public bool IsAutomated { get; set; } 

   

    public ICollection<TestStepDTO> TestSteps { get; set; } = new List<TestStepDTO>();
}

}
