using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using OrganizationManagement.DBContext;
using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using System.Threading.Tasks;

namespace OrganizationManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _tables;

        public AccountController(ApplicationDbContext tables)
        {
            _tables = tables;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View(new AdminDto());
        }

        [HttpPost]
        public async Task<IActionResult> Register(AdminDto model)
        {
            Console.WriteLine("1");
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Key: {entry.Key}, Error: {error.ErrorMessage}");
                    }
                }
                return View(model);
            }
            Console.WriteLine("2");

            var existing = await _tables.Admins.FirstOrDefaultAsync(a => a.Email == model.Email);
            if (existing != null)
            {
                ModelState.AddModelError("", "Email is already registered.");
                return View(model);
           }
            Console.WriteLine("3");
            var passwordHasher = new PasswordHasher<Admin>();
            var admin = new Admin
            {
                Name = model.Name,
                Email = model.Email,
                Role = model.Role,
                Password = passwordHasher.HashPassword(null, model.Password)
            };
            Console.WriteLine("4");
            await _tables.Admins.AddAsync(admin);
            await _tables.SaveChangesAsync();

            return RedirectToAction("Login", "Account");
        }
    }
}
