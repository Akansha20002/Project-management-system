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
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TestPlan> TestsPlans { get; set; }
        public DbSet<TestSuite> TestSuites { get; set; }
        public DbSet<TestCase> TestCases { get; set; }
        public DbSet<TestStep> TestSteps { get; set; }
    }
    }
