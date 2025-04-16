using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OrganizationManagement.Models;
using OrganizationManagement.DBContext;
using OrganizationManagement.DTO;

namespace OrganizationManagement.Controllers
{
    [Authorize(AuthenticationSchemes = "CustomCookieAuth")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _tables;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ApplicationDbContext tables, ILogger<DashboardController> logger)
        {
            _tables = tables;
            _logger = logger;
        }

 
        public IActionResult Index()
        {
            var orgList = _tables.Organizations
                .Select(o => new OrganizationDTO
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToList();

            
            var model = new OrganizationDTO
            {
                Organizations = orgList
            };

            if (TempData["Success"] != null)
            {
                ViewBag.SuccessMessage = TempData["Success"].ToString();
            }

            ViewBag.ShowForm = false; 
            return View(model);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ShowRegisterForm()
        {
            ViewBag.ShowForm = true;

          
            var orgList = _tables.Organizations
                .Select(o => new OrganizationDTO
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToList();

            return View("Index", new OrganizationDTO
            {
                Organizations = orgList
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterName(OrganizationDTO model)
        {
            if (ModelState.IsValid)
            {
             
                var newOrg = new Organization
                {
                    Name = model.Name
                };

                _tables.Organizations.Add(newOrg);
                _tables.SaveChanges();

                TempData["Success"] = "Organization registered successfully!";
                return RedirectToAction("Index");
            }

            model.Organizations = _tables.Organizations
                .Select(o => new OrganizationDTO
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToList();

            ViewBag.ShowForm = true; 
            return View("Index", model);
        }

       
        public IActionResult ViewOrganizations()
        {
            var orgList = _tables.Organizations
                .Select(o => new OrganizationDTO
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToList();

            var model = new OrganizationDTO
            {
                Organizations = orgList
            };

            return PartialView("_OrganizationsList", model); 
        }
    }
}
