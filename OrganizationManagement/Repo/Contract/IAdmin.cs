namespace OrganizationManagement.Repo.Contract
{
    public interface IAdmin
    {
        Task<IAdmin> GetAdminByIdAsync(int id);
        Task<IQueryable<IAdmin>> GetAllAdminsAsync();
        Task<IAdmin> CreateAdminAsync(IAdmin admin);
        Task<IAdmin> UpdateAdminAsync(IAdmin admin);
        Task<bool> DeleteAdminAsync(int id);
    }
}
