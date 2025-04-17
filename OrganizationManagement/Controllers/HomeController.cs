using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using OrganizationManagement.Services.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OrganizationManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAdminService _adminService;
        private readonly IOrganizationService _organizationService;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IAdminService adminService, IOrganizationService organizationService)
        {
            _adminService = adminService;
            _organizationService = organizationService;
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

        [HttpPost]
        public async Task<IActionResult> Login(AdminDto admin)
        {
            // Authenticate the admin user using AdminService
            var user = _adminService.AuthenticateUser(admin);

            if (user == null)
            {
                TempData["Error"] = "Invalid username or password";
                return RedirectToAction("Login");
            }

            // Create claims and sign in
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

        [Authorize(AuthenticationSchemes = "CustomCookieAuth")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "CustomCookieAuth")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CustomCookieAuth");
            return RedirectToAction("Login");
        }
    }
}
