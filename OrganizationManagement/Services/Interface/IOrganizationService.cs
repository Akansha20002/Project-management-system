using OrganizationManagement.Models;

namespace OrganizationManagement.Services.Interface
{
    public interface IOrganizationService
    {
        ICollection<Organization> GetOrganizations();

        ICollection<Organization> GetOrganizationsByUserId(int userId);
        Organization Add(Organization org);
        Organization Update(Organization org);
        Organization Delete(Organization org);
        bool OrganizationExists(int userId, string orgName);





    }
}
