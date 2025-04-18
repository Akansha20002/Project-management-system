using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using OrganizationManagement.DBContext;
using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using System.Linq;
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

        [HttpGet]
        public IActionResult Login()
        {
            return View(new AdminDto());
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminDto model)
        {
            var user = await _tables.Admins.FirstOrDefaultAsync(a => a.Email == model.Email);
            if (user == null || user.Role != "user")
            {
                ModelState.AddModelError("", "Invalid email or unauthorized role.");
                return View(model);
            }

            var passwordHasher = new PasswordHasher<Admin>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Invalid password.");
                return View(model);
            }

            return RedirectToAction("PostLoginOptions", new { userId = user.Id });
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new AdminDto());
        }

        [HttpPost]
        public async Task<IActionResult> Register(AdminDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existing = await _tables.Admins.FirstOrDefaultAsync(a => a.Email == model.Email);
            if (existing != null)
            {
                ModelState.AddModelError("", "Email already registered.");
                return View(model);
            }

            if (model.Role != "user")
            {
                ModelState.AddModelError("", "Only role 'user' is allowed.");
                return View(model);
            }

            var passwordHasher = new PasswordHasher<Admin>();
            var admin = new Admin
            {
                Name = model.Name,
                Email = model.Email,
                Role = model.Role,
                Password = passwordHasher.HashPassword(null, model.Password)
            };

            await _tables.Admins.AddAsync(admin);
            await _tables.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> PostLoginOptions(int userId)
        {
            var user = await _tables.Admins.FindAsync(userId);
            if (user == null || user.Role != "user")
            {
                return RedirectToAction("Login");
            }

            ViewBag.UserId = userId;
            ViewBag.UserName = user.Name;

            var organizations = await _tables.Organizations
                                             .Where(o => o.CreatedBy == userId)
                                             .ToListAsync();
            ViewBag.Organizations = organizations;

            return View();
        }

        [HttpGet]
        public IActionResult RegisterName(int userId)
        {
            ViewBag.UserId = userId;
            return View(new OrganizationDTO());
        }

        [HttpPost]
        public async Task<IActionResult> RegisterName(int userId, OrganizationDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UserId = userId;
                return View(model);
            }

            var alreadyExists = await _tables.Organizations
                .AnyAsync(o => o.CreatedBy == userId && o.Name == model.Name);

            if (alreadyExists)
            {
                ModelState.AddModelError("", "You have already registered an organization with this name.");
                ViewBag.UserId = userId;
                return View(model);
            }

            var organization = new Organization
            {
                Name = model.Name,
                CreatedBy = userId
            };

            await _tables.Organizations.AddAsync(organization);
            await _tables.SaveChangesAsync();

            return RedirectToAction("PostLoginOptions", new { userId = userId });
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}
