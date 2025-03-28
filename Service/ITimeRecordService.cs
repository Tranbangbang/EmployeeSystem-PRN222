using EmployManager.Models;

namespace EmployManager.Service
{
    public interface ITimeRecordService
    {
        Task<List<TimeRecord>> GetTimeRecordsByEmployeeIdAsync(int employeeId);
        Task<List<TimeRecord>> GetTimeRecordsByMonthAsync(int employeeId, int month, int year);
        Task<TimeRecord> GetOpenTimeRecordAsync(int employeeId);
        Task<bool> CheckInAsync(int employeeId, string notes = null);
        Task<bool> CheckOutAsync(int employeeId);
        Task<TimeRecord> GetTimeRecordByIdAsync(int id);
        Task<bool> UpdateTimeRecordAsync(TimeRecord timeRecord);
        Task<bool> DeleteTimeRecordAsync(int id);
    }
}
