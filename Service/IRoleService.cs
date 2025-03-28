using EmployManager.Models;

namespace EmployManager.Service
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int id);
    }
}
