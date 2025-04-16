using System.ComponentModel.DataAnnotations;

namespace OrganizationManagement.DTO
{
    public class TestPlanDTO
    {
            public int TestPlanId { get; set; }

            
            [MaxLength(10, ErrorMessage = "Test Plan name can't exceed 10 characters.")]
            public string Name { get; set; }

        
            [MaxLength(500, ErrorMessage = "Objective can't exceed 500 characters.")]
            public string Objective { get; set; }

         
            public string CreatedBy { get; set; }

           
            public string Strategy { get; set; }

       
            public int ProjectId { get; set; }

            // Optionally, you can include TestSuites here
            public ICollection<TestSuiteDTO> TestSuites { get; set; }
        }

    
}


