using System.ComponentModel.DataAnnotations;

namespace OrganizationManagement.Models
{
    public class Organization
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}
