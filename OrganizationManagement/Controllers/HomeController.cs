//using Microsoft.AspNetCore.Mvc;
//using OrganizationManagement.DTO;
//using OrganizationManagement.DBContext;
//using System.Security.Claims;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.EntityFrameworkCore;

//namespace OrganizationManagement.Controllers
//{
//    public class HomeController : Controller
//    {

//        private readonly ILogger<HomeController> _logger;
//        private readonly IConfiguration _configuration;
//        private readonly ApplicationDbContext _tables;




//        public HomeController( ILogger<HomeController> logger,IConfiguration configuration, ApplicationDbContext tables)
//        {
//            _tables = tables;
//            _logger = logger;
//            _configuration = configuration;
//        }
//        public IActionResult Index()
//        {
//            return View();
//        }
//        public IActionResult Login()
//        {
//            return View();
//        }

//        public IActionResult Create()
//        {
//            return View();
//        }

//        public IActionResult Register()
//        {
//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> Login(AdminDto admin)
//        {
//            var user = await _tables.Admins
//            .FirstOrDefaultAsync(x => x.Email == admin.Email);

//            if (user == null || admin.Password!=user.Password)
//            {
//                TempData["Error"] = "Invalid username or password";
//                return RedirectToAction("Login");
//            }

//            var claims = new List<Claim>
//     {
//         new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//         new Claim(ClaimTypes.Name, user.Name),
//         new Claim("Role", "Admin")
//     };

//            var claimsIdentity = new ClaimsIdentity(claims, "CustomCookieAuth");
//            var authProperties = new AuthenticationProperties
//            {
//                IsPersistent = true, 
//                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
//            };

//            await HttpContext.SignInAsync("CustomCookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

//            return RedirectToAction("Index","Dashboard");
//        }
//        [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = "CustomCookieAuth")]
//        public IActionResult Dashboard()
//        {
//            return View();
//        }
//        [HttpPost]
//        [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = "CustomCookieAuth")]
//        public async Task<IActionResult> Logout()
//        {
//            await HttpContext.SignOutAsync("CustomCookieAuth");
//            return RedirectToAction("Login");
//        }


//    }
//}
using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.DTO;
using OrganizationManagement.DBContext;
using OrganizationManagement.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace OrganizationManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _tables;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, ApplicationDbContext tables)
        {
            _tables = tables;
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Admin admin)
        {
            if (!ModelState.IsValid)
                return View(admin);

            // Allow only Admin or User roles
            if (admin.Role != "Admin" && admin.Role != "User")
            {
                ModelState.AddModelError("Role", "Only 'Admin' or 'User' roles are allowed.");
                return View(admin);
            }

            // Check if email is already registered
            var existing = await _tables.Admins.FirstOrDefaultAsync(x => x.Email == admin.Email);
            if (existing != null)
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(admin);
            }

            _tables.Admins.Add(admin);
            await _tables.SaveChangesAsync();

            TempData["Success"] = "Registration successful. Please login.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminDto admin)
        {
            var user = await _tables.Admins
                .FirstOrDefaultAsync(x => x.Email == admin.Email);

            if (user == null || admin.Password != user.Password)
            {
                TempData["Error"] = "Invalid username or password";
                return RedirectToAction("Login");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("Role", user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "CustomCookieAuth");
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
            };

            await HttpContext.SignInAsync("CustomCookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Index", "Dashboard");
        }

        [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = "CustomCookieAuth")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = "CustomCookieAuth")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CustomCookieAuth");
            return RedirectToAction("Login");
        }
    }
}

