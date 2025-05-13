
using System.Collections.Generic;

namespace OrganizationManagement.DTO
{
    public class OrganizationDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProjectDTO> Projects { get; set; } = new List<ProjectDTO>();
    }

    
}
