using OrganizationManagement.Models;

namespace OrganizationManagement.Repo.Contract
{
    public interface IOrganization
    {
        public ICollection<Models.Organization> GetOrganizations(); //to get the organization from database

        public Models.Organization AddOrganization(Models.Organization org);

        public Models.Organization UpdateOrganization(Models.Organization updatedorg);


        public bool DeleteOrganization(int organizationId); //delete the org by its ID

    }
}
