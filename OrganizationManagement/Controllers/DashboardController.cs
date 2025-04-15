
using Microsoft.AspNetCore.Mvc;
using OrganizationManagement.DTO;
using OrganizationManagement.Repo.Contract;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace OrganizationManagement.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IOrganization _orgrepository;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IOrganization orgrepository, ILogger<DashboardController> logger)
        {
            _orgrepository = orgrepository;
            _logger = logger;
        }


        [Authorize(AuthenticationSchemes = "CustomCookieAuth")]
        public IActionResult Index()
        {
            //var adminUsername = Request.Cookies["AdminUsername"];
            var adminUsername = User.Identity?.Name;

            // Check if the admin username is missing in cookies and redirect to login
            if (string.IsNullOrEmpty(adminUsername))
            {
                _logger.LogWarning("AdminUsername cookie is missing.");
                return RedirectToAction("Login", "Home");
            }

            var organization = _orgrepository.GetOrganizations();

            // Ensure that organizationInfo is always initialized, even if empty
            var organizationInfo = organization?.Select(o => new OrganizationDTO
            {
                Name = o.Name
            }).ToList() ?? new List<OrganizationDTO>();

       
            var dashboardDTO = new DashboardDTO
            {
                AdminName = adminUsername,
                organizationInfo = organizationInfo
            };

            // Log the dashboardDTO object to make sure it's created properly
            _logger.LogInformation("DashboardDTO created: AdminName = {AdminName}, Organizations Count = {OrgCount}",
                dashboardDTO.AdminName, dashboardDTO.organizationInfo.Count);

            return View(dashboardDTO); // Pass the model to the view
        }
    }
}


