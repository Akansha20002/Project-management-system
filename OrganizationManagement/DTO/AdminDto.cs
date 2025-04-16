using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace OrganizationManagement.DTO
{
    public class AdminDto
    {
             public int Id { get; set; }

            [Required, MaxLength(50)]
            public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

            [Required]
            [StringLength(100, MinimumLength = 8)]
            [DataType(DataType.Password)]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
                ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character.")]
            public string Password { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public string Role {  get; set; }


        [ValidateNever]
        public ICollection<OrganizationDTO> Organizations { get; set; }
        }
    }
