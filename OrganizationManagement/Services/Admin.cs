using Microsoft.AspNetCore.Identity;
using OrganizationManagement.DBContext;
using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using OrganizationManagement.Services.Interface;
using System.Linq;

namespace OrganizationManagement.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Admin AuthenticateUser(AdminDto model)
        {
            var user = _dbContext.Admins.FirstOrDefault(a => a.Email == model.Email);
            if (user == null || user.Role != "user")
                return null;

            var passwordHasher = new PasswordHasher<Admin>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

            if (result == PasswordVerificationResult.Failed)
                return null;

            return user; // Return user if authenticated successfully
        }

        public bool RegisterUser(AdminDto model)
        {
            // Check if email already exists
            var existingUser = _dbContext.Admins.FirstOrDefault(a => a.Email == model.Email);
            if (existingUser != null)
                return false;

            var passwordHasher = new PasswordHasher<Admin>();
            var newAdmin = new Admin
            {
                Name = model.Name,
                Email = model.Email,
                Role = model.Role,
                Password = passwordHasher.HashPassword(null, model.Password)
            };

            _dbContext.Admins.Add(newAdmin);
            _dbContext.SaveChanges();

            return true; // Return true if registration was successful
        }

        public Admin GetAdminById(int userId)
        {
            // Fetch admin by ID
            return _dbContext.Admins.FirstOrDefault(a => a.Id == userId);
        }
    }
}
