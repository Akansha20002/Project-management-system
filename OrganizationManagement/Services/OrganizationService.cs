using OrganizationManagement.Models;
using OrganizationManagement.Repo.Contract;
using OrganizationManagement.Services.Interface;

namespace OrganizationManagement.Services
{
    public class OrganizationService : IOrganizationService
    {
        IOrganizationRepository _repository;
        public OrganizationService(IOrganizationRepository repository) {
            _repository = repository;
        }
        public Organization Add(Organization org)
        {
            return _repository.Add(org);
        }

        public Organization Delete(Organization org)
        {
            return _repository.Delete(org);
        }

        public ICollection<Organization> GetOrganizations()
        {
            return _repository.GetOrganizations();
        }

        public ICollection<Organization> GetOrganizationsByUserId(int userId)
        {
            return _repository.GetOrganizationsByUserId(userId);  // Fetch organizations for a specific user
        }

        public bool OrganizationExists(int userId, string orgName)
        {
            return _repository.OrganizationExists(userId, orgName);
        }


        public Organization Update(Organization org)
        {
           return (_repository.Update(org));
        }
    }
}
