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
        public string Role { get; set; }
        public ICollection<Organization> Organizations { get; set; }
    }

    


}


