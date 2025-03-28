using EmployManager.Models;

namespace EmployManager.Service.impl
{
    public interface IEmployeeProfileService
    {
        Task<EmployeeProfile> GetProfileByEmployeeIdAsync(int employeeId);
        Task<bool> CreateProfileAsync(EmployeeProfile profile);
        Task<bool> UpdateProfileAsync(EmployeeProfile profile);
        Task<bool> UploadProfileImageAsync(int employeeId, IFormFile file);
        Task<string> GetProfileImagePathAsync(int employeeId);
    }
}
