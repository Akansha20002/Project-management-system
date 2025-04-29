using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.DTO;
using OrganizationManagement.Services;
using System;
using System.Threading.Tasks;

namespace OrganizationManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (Request.Cookies.ContainsKey("UserId"))
            {
                int userId = int.Parse(Request.Cookies["UserId"]);
                return RedirectToAction("PostLoginOptions", new { userId = userId });
            }

            return View(new AdminDto());
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminDto model)
        {
            var user = await _accountService.AuthenticateUserAsync(model);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid credentials or unauthorized role.");
                return View(model);
            }

            Response.Cookies.Append("UserId", user.Id.ToString(), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(1) });
            Response.Cookies.Append("UserName", user.Name, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(1) });

            return RedirectToAction("PostLoginOptions", new { userId = user.Id });
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (Request.Cookies.ContainsKey("UserId"))
            {
                int userId = int.Parse(Request.Cookies["UserId"]);
                return RedirectToAction("PostLoginOptions", new { userId = userId });
            }

            return View(new AdminDto());
        }

        [HttpPost]
        public async Task<IActionResult> Register(AdminDto model)
        {
            if (Request.Cookies.ContainsKey("UserId"))
            {
                int userId = int.Parse(Request.Cookies["UserId"]);
                return RedirectToAction("PostLoginOptions", new { userId = userId });
            }

            if (!ModelState.IsValid)
                return View(model);

            if (await _accountService.IsEmailRegisteredAsync(model.Email))
            {
                ModelState.AddModelError("", "Email already registered.");
                return View(model);
            }

            if (model.Role != "user")
            {
                ModelState.AddModelError("", "Only role 'user' is allowed.");
                return View(model);
            }

            await _accountService.RegisterUserAsync(model);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> PostLoginOptions(int userId)
        {
            var user = await _accountService.GetUserByIdAsync(userId);
            if (user == null || user.Role != "user")
            {
                return RedirectToAction("Login");
            }

            if (!Request.Cookies.ContainsKey("UserId"))
            {
                Response.Cookies.Append("UserId", user.Id.ToString(), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(1) });
                Response.Cookies.Append("UserName", user.Name, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(1) });
            }

            ViewBag.UserId = userId;
            ViewBag.UserName = user.Name;
            ViewBag.Organizations = await _accountService.GetOrganizationsForUserAsync(userId);

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

            if (await _accountService.OrganizationExistsAsync(userId, model.Name))
            {
                ModelState.AddModelError("", "You have already registered an organization with this name.");
                ViewBag.UserId = userId;
                return View(model);
            }

            await _accountService.RegisterOrganizationAsync(userId, model);
            return RedirectToAction("PostLoginOptions", new { userId = userId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrganization(int id)
        {
            await _accountService.DeleteOrganizationAsync(id);
            int userId = int.Parse(Request.Cookies["UserId"]);
            return RedirectToAction("PostLoginOptions", new { userId = userId });
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("UserId");
            Response.Cookies.Delete("UserName");
            return RedirectToAction("Login");
        }
    }
}
