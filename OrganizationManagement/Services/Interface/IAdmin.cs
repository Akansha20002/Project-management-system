using OrganizationManagement.DTO;
using OrganizationManagement.Models;
using System.Collections.Generic;

namespace OrganizationManagement.Services.Interface
{
    public interface IAdminService
    {
        Admin AuthenticateUser(AdminDto model);   // authenticate user
        bool RegisterUser(AdminDto model);         // register a user
        Admin GetAdminById(int userId);            //  get admin by ID
    }
}
