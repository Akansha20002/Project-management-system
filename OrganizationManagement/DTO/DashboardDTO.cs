namespace OrganizationManagement.DTO
{
    public class DashboardDTO
    {
        public string AdminName { get; set; }

        public ICollection<OrganizationDTO> organizationInfo { get; set; } // List of OrganizationDTO
    }
}
