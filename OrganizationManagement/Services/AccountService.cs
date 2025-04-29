using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrganizationManagement.DBContext;
using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using System;
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

        public async Task<Admin> AuthenticateUserAsync(AdminDto model)
        {
            var user = await _context.Admins.FirstOrDefaultAsync(a => a.Email == model.Email);
            if (user == null || user.Role != "user")
                return null;

            var passwordHasher = new PasswordHasher<Admin>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

            return result == PasswordVerificationResult.Success ? user : null;
        }

        public async Task<bool> IsEmailRegisteredAsync(string email)
        {
            return await _context.Admins.AnyAsync(a => a.Email == email);
        }

        public async Task<Admin> RegisterUserAsync(AdminDto model)
        {
            var passwordHasher = new PasswordHasher<Admin>();
            var admin = new Admin
            {
                Name = model.Name,
                Email = model.Email,
                Role = model.Role,
                Password = passwordHasher.HashPassword(null, model.Password)
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();
            return admin;
        }

        public async Task<Admin> GetUserByIdAsync(int userId)
        {
            return await _context.Admins.FindAsync(userId);
        }

        public async Task<List<Organization>> GetOrganizationsForUserAsync(int userId)
        {
            return await _context.Organizations
                                 .Where(o => o.CreatedBy == userId)
                                 .ToListAsync();
        }

        public async Task<bool> OrganizationExistsAsync(int userId, string orgName)
        {
            return await _context.Organizations
                .AnyAsync(o => o.CreatedBy == userId && o.Name == orgName);
        }

        public async Task RegisterOrganizationAsync(int userId, OrganizationDTO model)
        {
            var org = new Organization
            {
                Name = model.Name,
                CreatedBy = userId
            };

            await _context.Organizations.AddAsync(org);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrganizationAsync(int organizationId)
        {
            var org = await _context.Organizations.FindAsync(organizationId);
            if (org != null)
            {
                _context.Organizations.Remove(org);
                await _context.SaveChangesAsync();
            }
        }
    }
}
