using System.ComponentModel.DataAnnotations;


namespace OrganizationManagement.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; } 

        [Required]
        [StringLength(100, MinimumLength = 8)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character.")]
        public string Password { get; set; }
        public ICollection<Organization> Organizations { get; set; }
    }

    //public class Project
    //{
    //    public int ProjectId { get; set; }

    //    [Required]
    //    public string ProjectName { get; set; }

    //    [Required]
    //    public string Status { get; set; }

    //    [Required, MaxLength(500)]
    //    public string Description { get; set; }

    //    [Required]
    //    public DateTime StartDate { get; set; }

    //    [Required]
    //    public DateTime EndDate { get; set; }

    //    public int OrganizationId { get; set; }

    //    public ICollection<TestPlan> TestPlans { get; set; }
    //}


}


