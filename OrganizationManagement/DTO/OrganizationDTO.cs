using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OrganizationManagement.DTO
{
    public class OrganizationDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Organization name is required.")]
        public string Name { get; set; }

        [BindNever] // Tells ASP.NET to skip binding and validation for this property
        public ICollection<ProjectDTO>? Projects { get; set; }

        public List<OrganizationDTO> Organizations { get; set; } = new List<OrganizationDTO>();
    }
}
