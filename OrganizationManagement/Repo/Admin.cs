namespace OrganizationManagement.Repo
{
    public interface Admin
    {
        Task<Admin> GetAdminByIdAsync(int id);
        Task<IQueryable<Admin>> GetAllAdminsAsync();
        Task<Admin> CreateAdminAsync(Admin admin);
        Task<Admin> UpdateAdminAsync(Admin admin);
        Task<bool> DeleteAdminAsync(int id);
    }
}
