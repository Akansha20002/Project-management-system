using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.DTO;

using OrganizationManagement.DBContext;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace OrganizationManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger,IConfiguration configuration)
        {
            _context = context;
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

        public IActionResult Login(AdminDto admin)
        {
            var existingAdmin = _context.Admins
                .FirstOrDefault(a => a.Email == admin.Email && a.Password == admin.Password);

            if (existingAdmin != null)
            {
                var token = GenerateJwtToken(admin);
                ViewBag.token = token;
                return View("Dashboard"); 
            }

            ModelState.AddModelError(string.Empty, "Invalid login");
            return View();
        }
        private string GenerateJwtToken(AdminDto admin)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, admin.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
               
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [Authorize]
        public IActionResult Dashboard()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminToken");
            return RedirectToAction("AdminLogin");
        }
    }
}
