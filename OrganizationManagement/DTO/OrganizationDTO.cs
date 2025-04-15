namespace OrganizationManagement.DTO
{
    public class OrganizationDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public ICollection<ProjectDTO> Projects{ get; set; }
    public List<OrganizationDTO> Organizations { get; set; } = new List<OrganizationDTO>();
    }
}
