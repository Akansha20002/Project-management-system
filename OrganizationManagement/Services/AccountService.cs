using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrganizationManagement.DBContext;
using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using OrganizationManagement.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrganizationManagement.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Admin> LoginAsync(AdminDto model)
        {
            var user = await _context.Admins.FirstOrDefaultAsync(a => a.Email == model.Email);
            if (user == null || user.Role != "user")
                return null;

            var hasher = new PasswordHasher<Admin>();
            var result = hasher.VerifyHashedPassword(user, user.Password, model.Password);

            return result == PasswordVerificationResult.Success ? user : null;
        }

        public async Task<bool> RegisterAsync(AdminDto model)
        {
            if (await _context.Admins.AnyAsync(a => a.Email == model.Email))
                return false;

            if (model.Role != "user")
                return false;

            var hasher = new PasswordHasher<Admin>();
            var admin = new Admin
            {
                Name = model.Name,
                Email = model.Email,
                Role = model.Role,
                Password = hasher.HashPassword(null, model.Password)
            };

            await _context.Admins.AddAsync(admin);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Admin> GetUserByIdAsync(int userId)
        {
            return await _context.Admins.FindAsync(userId);
        }

        public async Task<List<Organization>> GetOrganizationsByUserIdAsync(int userId)
        {
            return await _context.Organizations
                .Where(o => o.CreatedBy == userId)
                .ToListAsync();
        }

        public async Task<bool> RegisterOrganizationAsync(int userId, OrganizationDTO model)
        {
            if (await _context.Organizations
                .AnyAsync(o => o.CreatedBy == userId && o.Name == model.Name))
                return false;

            var organization = new Organization
            {
                Name = model.Name,
                CreatedBy = userId
            };

            await _context.Organizations.AddAsync(organization);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrganizationAsync(int orgId)
        {
            var org = await _context.Organizations.FindAsync(orgId);
            if (org == null)
                return false;

            _context.Organizations.Remove(org);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
