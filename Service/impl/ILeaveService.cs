using EmployManager.Models;

namespace EmployManager.Service.impl
{
    public interface ILeaveService
    {
        Task<List<Leave>> GetLeavesByEmployeeIdAsync(int employeeId);
        Task<List<Leave>> GetPendingLeavesAsync();
        Task<Leave> GetLeaveByIdAsync(int id);
        Task<bool> CreateLeaveAsync(Leave leave);
        Task<bool> UpdateLeaveAsync(Leave leave);
        Task<bool> ApproveLeaveAsync(int id, string managerComments);
        Task<bool> RejectLeaveAsync(int id, string managerComments);
        Task<bool> DeleteLeaveAsync(int id);
    }
}
