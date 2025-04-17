using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using OrganizationManagement.Services.Interface;

namespace OrganizationManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IOrganizationService _organizationService;

        public AccountController(IAdminService adminService, IOrganizationService organizationService)
        {
            _adminService = adminService;
            _organizationService = organizationService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new AdminDto());
        }

        [HttpPost]
        public IActionResult Login(AdminDto model)
        {
            var user = _adminService.AuthenticateUser(model);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or unauthorized role.");
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
        public IActionResult Register(AdminDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var isRegistered = _adminService.RegisterUser(model);
            if (!isRegistered)
            {
                ModelState.AddModelError("", "Email already registered.");
                return View(model);
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult PostLoginOptions(int userId)
        {
            var user = _adminService.GetAdminById(userId);
            if (user == null || user.Role != "user")
            {
                return RedirectToAction("Login");
            }

            ViewBag.UserId = userId;

            var organizations = _organizationService.GetOrganizationsByUserId(userId);  
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
        public IActionResult RegisterName(int userId, OrganizationDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UserId = userId;
                return View(model);
            }

            var alreadyExists = _organizationService.OrganizationExists(userId, model.Name);
            if (alreadyExists)
            {
                ModelState.AddModelError("", "You have already registered an organization.");
                ViewBag.UserId = userId;
                return View(model);
            }

            var organization = new Organization
            {
                Name = model.Name,
                CreatedBy = userId
            };

            _organizationService.Add(organization);
            return RedirectToAction("PostLoginOptions", new { userId = userId });
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}
