using OrganizationManagement.DBContext;
using OrganizationManagement.Models;
using OrganizationManagement.Repo.Contract;

namespace OrganizationManagement.Repo
{
    public class Organization : IOrganization
    {
        ApplicationDbContext _context;

        public Organization(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<Models.Organization> GetOrganizations()
        {
            return _context.Organizations.ToList();
            
        }

        public Models.Organization AddOrganization(Models.Organization org)
        {
            _context.Organizations.Add(org);
            _context.SaveChanges();
            return (org);
        }

        public Models.Organization UpdateOrganization(Models.Organization updatedorg)
        {
            var existedorg= _context.Organizations.FirstOrDefault(i=>i.Id==updatedorg.Id);
            if (existedorg == null)
            {
                return null;
            }

            existedorg.Name = updatedorg.Name;
            _context.SaveChanges();
            return existedorg;

        }

        public bool DeleteOrganization(int organizationId)
        {
            var org = _context.Organizations.FirstOrDefault(o => o.Id==organizationId);
            if (org == null) {
                return false;
            }

            _context.Organizations.Remove(org);
            _context.SaveChanges(true);
            return true;
            
        }

      
    }
}
