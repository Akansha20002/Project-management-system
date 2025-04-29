using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrganizationManagement.Services
{
    public interface IAccountService
    {
        Task<Admin> AuthenticateUserAsync(AdminDto model);
        Task<bool> IsEmailRegisteredAsync(string email);
        Task<Admin> RegisterUserAsync(AdminDto model);
        Task<Admin> GetUserByIdAsync(int userId);
        Task<List<Organization>> GetOrganizationsForUserAsync(int userId);
        Task<bool> OrganizationExistsAsync(int userId, string orgName);
        Task RegisterOrganizationAsync(int userId, OrganizationDTO model);
        Task DeleteOrganizationAsync(int organizationId);
    }
}
