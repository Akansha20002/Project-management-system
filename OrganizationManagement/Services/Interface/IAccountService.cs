using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrganizationManagement.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Admin> LoginAsync(AdminDto model);
        Task<bool> RegisterAsync(AdminDto model);
        Task<Admin> GetUserByIdAsync(int userId);
        Task<List<Organization>> GetOrganizationsByUserIdAsync(int userId);
        Task<bool> RegisterOrganizationAsync(int userId, OrganizationDTO model);
        Task<bool> DeleteOrganizationAsync(int orgId);
    }
}
