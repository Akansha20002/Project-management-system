using OrganizationManagement.Models;
using System.Collections.Generic;

namespace OrganizationManagement.Repo.Contract
{
    public interface IOrganizationRepository
    {
        ICollection<Organization> GetOrganizations();
        Organization Add(Organization org);
        Organization Update(Organization org);
        Organization Delete(Organization org);
        bool OrganizationExists(int userId, string orgName);
        ICollection<Organization> GetOrganizationsByUserId(int userId);

    }
}
