using Microsoft.EntityFrameworkCore;
using OrganizationManagement.Models;

namespace OrganizationManagement.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Admin> Admins { get; set; }
    }
    }
