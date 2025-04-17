using OrganizationManagement.DBContext;
using OrganizationManagement.Models;
using OrganizationManagement.Repo.Contract;
using System.Collections.Generic;

namespace OrganizationManagement.Repo
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrganizationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Organization Add(Organization org)
        {
            _dbContext.Organizations.Add(org);
            _dbContext.SaveChanges();
            return org;
        }

        public Organization Delete(Organization org)
        {
            _dbContext.Organizations.Remove(org);
            _dbContext.SaveChanges();
            return org;
        }

        public ICollection<Organization> GetOrganizations()
        {
            return _dbContext.Organizations.ToList();
        }

        public ICollection<Organization> GetOrganizationsByUserId(int userId)
        {
            return _dbContext.Organizations
                             .Where(o => o.CreatedBy == userId)
                             .ToList();
        }

        public bool OrganizationExists(int userId, string orgName)
        {
            return _dbContext.Organizations
                             .Any(o => o.CreatedBy == userId && o.Name == orgName);
        }


        public Organization Update(Organization org)
        {
            _dbContext.Organizations.Update(org);
            _dbContext.SaveChanges();
            return org;
        }
    }
}
