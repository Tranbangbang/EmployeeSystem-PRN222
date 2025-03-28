using EmployManager.Models;

namespace EmployManager.Service
{
    public interface IOvertimeService
    {
        Task<List<OvertimeRequest>> GetOvertimeRequestsByEmployeeIdAsync(int employeeId);
        Task<List<OvertimeRequest>> GetPendingOvertimeRequestsAsync();
        Task<OvertimeRequest> GetOvertimeRequestByIdAsync(int id);
        Task<bool> CreateOvertimeRequestAsync(OvertimeRequest overtimeRequest);
        Task<bool> UpdateOvertimeRequestAsync(OvertimeRequest overtimeRequest);
        Task<bool> ApproveOvertimeRequestAsync(int id, string managerComments);
        Task<bool> RejectOvertimeRequestAsync(int id, string managerComments);
        Task<bool> DeleteOvertimeRequestAsync(int id);
    }
}
